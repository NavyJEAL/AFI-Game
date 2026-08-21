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
	private readonly PlayerService _playerService;
	public GameAPIController(AppDbContext context, GameService gameService, IHubContext<GameHub> hubContext, MovieCardService movieCardService, PlayerService playerService)
	{
		_context = context;
		_gameService = gameService;
		_playerService = playerService;
		_hubContext = hubContext;
		_movieCardService = movieCardService;
	}

	[HttpPost("create")]
	public async Task<IActionResult> CreateGame( [FromBody] CreateGameRequest request)
	{
		var player = _gameService.CreateGame(request.AccountID);
		await _hubContext.Clients.Group("lobby-list").SendAsync("GameListUpdated");

		return Ok(new { gameId = player.GameID, playerId = player.PlayerID });
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

	[HttpPost("{gameId}/leave")]
	public async Task<IActionResult> LeaveGame(int gameId, [FromBody] LeaveGameRequest request)
	{
		try
		{
			_gameService.LeaveGame(gameId, request.AccountID);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		await _hubContext.Clients.Group($"game-{gameId}")
			.SendAsync("PlayerLeft", new {accountId = request.AccountID});
		
		await _hubContext.Clients.Group("lobby-list").SendAsync("GameListUpdated");
		
		return Ok(new { message = "Player left the game" });
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
	public async Task<IActionResult> PlaceCard(int gameId, [FromBody] PlaceCardRequest request)
	{
		bool isCorrect;
		try
		{
			isCorrect = _gameService.PlaceCard(request.PlayerId, request.CardId, request.Position);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}

		if (isCorrect)
		{
			await _hubContext.Clients.Group($"game-{gameId}")
				.SendAsync("CardPlaced", new
				{
					playerId = request.PlayerId,
					cardId = request.CardId,
					correct = true
				});
		}
		else
		{
			var game = _gameService.AdvanceTurn(gameId);

			await _hubContext.Clients.Group($"game-{gameId}")
				.SendAsync("TurnChanged", new
				{
					currentPlayerId = game.CurrentPlayerID
				});
		}

		return Ok(new { correct = isCorrect });
	}

	[HttpGet("{gameId}/player/{playerId}/timeline")]
	public IActionResult GetTimeline(int gameId, int playerId)
	{
		var timeline = _playerService.GetPlayerTimeline(playerId)
			.Select(pc => new
			{
				position = pc.Position,
				title = pc.MovieCard.Title,
				releaseYear = pc.MovieCard.ReleaseYear
			})
			.ToList();

		return Ok(timeline);
	}

}