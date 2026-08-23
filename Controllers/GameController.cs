using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.Services;

namespace AFI_Game.Controllers;
public class GameController : Controller
{
	private readonly AppDbContext _context;
	private readonly GameService _gameService;
	private readonly PlayerService _playerService;
	public GameController(AppDbContext context, GameService gameService, PlayerService playerService)
	{
		_context = context;
		_gameService = gameService;
		_playerService = playerService;
	}
	public IActionResult Lobby(int id)
	{
		var game = _gameService.GetGameByID(id);
		if (game == null) return NotFound();

		var accountIdString = User.FindFirst("AccountID")?.Value;
		ViewBag.AccountID = int.Parse(accountIdString);

		return View(game);
	}

	[HttpGet("Game/Play/{gameId}")]
	public IActionResult Play(int gameID)
	{
		Console.WriteLine($"Play called with gameId: {gameID}");
		var game = _gameService.GetGameByID(gameID);

		if (game == null)
		{
			return NotFound();
		}

        var accountIDString = User.FindFirst("AccountID")?.Value;
        int accountID = int.Parse(accountIDString);

        var player = _playerService.GetPlayerByAccount(gameID, accountID);
        if (player == null)
        {
            return Forbid();
        }

		var currentPlayer = game.Players.FirstOrDefault(p => p.PlayerID == game.CurrentPlayerID);

        ViewBag.PlayerID = player.PlayerID;
		ViewBag.CurrentPlayerID = game.CurrentPlayerID;
		ViewBag.CurrentPlayer = currentPlayer?.Account?.Username ?? "Unknown";
        return View(game);
    }
}