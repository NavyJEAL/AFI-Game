using System.Text.Json.Serialization;

namespace AFI_Game.DTO;
public class TMDBResponse
{
	public required List<TMDBMovie> Results { get; set; }
}
public class TMDBMovie
{
	public int Id { get; set; }
	public required string Title { get; set; }
	[JsonPropertyName("release_date")]
	public string? ReleaseDate { get; set; }
	[JsonPropertyName("genre_ids")]
	public List<int> GenreIds { get; set; } = [];
}

public class TMDBGenreResponse
{
	public required List<TMDBGenre> Genres { get; set; }
}

public class TMDBGenre
{
	public int Id { get; set; }
	public required string Name { get; set; }
}

public class TMDBCreditsResponse
{
	public List<TMDBCrewMember> Crew { get; set; } = [];
}

public class TMDBCrewMember
{
	public required string Name { get; set; }
	public required string Job { get; set; }
}
