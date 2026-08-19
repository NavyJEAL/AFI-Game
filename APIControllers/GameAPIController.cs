using Microsoft.AspNetCore.Mvc;
using AFI_Game.Services;
using Microsoft.AspNetCore.SignalR;
using AFI_Game.Hubs;
using AFI_Game.DTO;
using AFI_Game.Models;

[ApiController]
[Route("api/[controller]")]
public class GameAPIController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly GameService _gameService;
	private readonly IHubContext<GameHub> _hubContext;
	private readonly MovieCardService _movieCardService;
	public GameAPIController(AppDbContext context, GameService gameService, IHubContext<GameHub> hubContext, MovieCardService movieCardService)
	{
		_context = context;
		_gameService = gameService;
		_hubContext = hubContext;
		_movieCardService = movieCardService;
	}

	[HttpPost("create")]
	public IActionResult CreateGame()
	{
		var game = _gameService.CreateGame();
		return Ok(new { gameId = game.GameID });
	}

	[HttpPost("{gameId}/join")]
	public async Task<IActionResult> JoinGame(int gameId, [FromBody] JoinGameRequest request)
	{

		var player = _gameService.JoinGame(gameId, request.AccountID);

		await _hubContext.Clients.Group($"game-{gameId}")
			.SendAsync("PlayerJoined", new 
			{ 
				playerId = player.PlayerID, 
				username = player.Account.Username,
			});
		
		var game = _gameService.GetGameByID(gameId);
		return Ok(game);
	}

	[HttpPost("{gameId}/start")]
	public async Task<IActionResult> StartGame(int gameId)
	{
		Game game;
		try
		{
			game = _gameService.StartGame(gameId);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		// Meddela alla spelare i lobbyn att spelet nu startat
		await _hubContext.Clients.Group($"game-{gameId}")
			.SendAsync("GameStarted", new
			{
				gameId = game.GameID,
				currentPlayerId = game.CurrentPlayerID
			});

		return Ok(new
		{
			gameId = game.GameID,
			status = game.GameStatus,
			currentPlayerId = game.CurrentPlayerID
		});
	}

	[HttpGet("{gameId}/status")]
	public IActionResult GetStatusById(int gameId)
	{
		var currentGame = _gameService.GetGameByID(gameId);

		if (currentGame == null)
			return NotFound();

		return Ok(new
		{
			gameId = currentGame.GameID,
			gameStatus = currentGame.GameStatus,
			players = currentGame.Players.Select(p => new { p.PlayerID, p.TurnOrder, p.Points })
		});
	}

	[HttpPost("{gameId}/end")]
	public IActionResult EndGame(int gameId, [FromBody] EndGameRequest request)
	{
		var game = _gameService.EndGame(gameId, request.WinningPlayerID);

		_hubContext.Clients.Group($"game-{gameId}")
			.SendAsync("GameEnded", new { winnerId = request.WinningPlayerID });

		return Ok(new { gameId = game.GameID, status = game.GameStatus });
	}

	[HttpGet("{gameId}/drawcard")]
	public IActionResult DrawCard(int gameId)
	{
		var card = _movieCardService.GetRandomUnusedCard(gameId);

		if (card == null)
		{
			return NotFound("No more available cards");
		}

		return Ok(new
		{
			cardId = card.CardID,
			title = card.Title,
			director = card.Director,
		});
	}

	[HttpPost("{gameId}/placecard")]
	public IActionResult PlaceCard()
	{
		// Implementera logik för att placera kortet i spelet
		return Ok();
	}

}