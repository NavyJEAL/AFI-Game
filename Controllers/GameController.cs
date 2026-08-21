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
	public IActionResult Lobby(int ID)
	{
		var game = _gameService.GetGameByID(ID);

		if (game == null)
		{
			return NotFound();
		}

		return View(game);
	}

	public IActionResult Play(int gameID)
	{
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

        ViewBag.PlayerID = player.PlayerID;
		ViewBag.CurrentPlayerID = game.CurrentPlayerID;
        return View(game);
    }
}