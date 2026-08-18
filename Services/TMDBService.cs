using AFI_Game.Models;
using AFI_Game.DTO;
using Microsoft.EntityFrameworkCore;

public class TMDBService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly AppDbContext _context;
    private Dictionary<int, string> _genreMap = new();

    public TMDBService(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
    {
        _httpClient = httpClient;
        _apiKey = configuration["TMDB:ApiKey"] ?? throw new ArgumentNullException("TMDB API key is not configured.");
        _context = context;
    }

	// Seeds movie details from TMDB API into the database
    public async Task<string> SeedMovieDetails(int numberOfPages = 8)
    {
		Console.WriteLine($"TMDB API Key loaded: '{_apiKey}'");
        await LoadGenresAsync();

        for (int page = 1; page <= numberOfPages; page++)
        {
            var response = await _httpClient.GetAsync(
                $"https://api.themoviedb.org/3/discover/movie?api_key={_apiKey}&sort_by=popularity.desc&page={page}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieResponse = await response.Content.ReadFromJsonAsync<TMDBResponse>();

            if (movieResponse?.Results == null) continue;

            foreach (var movie in movieResponse.Results)
            {
                var releaseYear = DateTime.TryParse(movie.ReleaseDate, out var date) ? date.Year : 0;

                var existingMovie = await _context.MovieCard
                    .FirstOrDefaultAsync(m => m.Title == movie.Title && m.ReleaseYear == releaseYear);

                if (existingMovie == null)
                {
                    var director = await GetDirectorAsync(movie.Id);
                    var genreName = movie.GenreIds?.FirstOrDefault() is int gid && _genreMap.ContainsKey(gid)
                        ? _genreMap[gid]
                        : "Unknown";

                    var card = new MovieCard
                    {
                        Title = movie.Title,
                        Director = director,
                        ReleaseYear = releaseYear,
                        Genre = genreName,
                    };
                    _context.MovieCard.Add(card);
                }
            }
            await _context.SaveChangesAsync();
            await Task.Delay(300);
        }
        return "Movie details seeded successfully.";
    }

	// Loads genre mappings from TMDB API into a dictionary
    private async Task LoadGenresAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<TMDBGenreResponse>(
            $"https://api.themoviedb.org/3/genre/movie/list?api_key={_apiKey}");
        _genreMap = response?.Genres?.ToDictionary(g => g.Id, g => g.Name) ?? new();
    }

	// Fetches the director's name for a given movie ID from TMDB API
    private async Task<string> GetDirectorAsync(int movieId)
    {
        var response = await _httpClient.GetFromJsonAsync<TMDBCreditsResponse>(
            $"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key={_apiKey}");
        var director = response?.Crew?.FirstOrDefault(c => c.Job == "Director");
        return director?.Name ?? "Unknown";
    }
}