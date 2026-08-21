using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.Services;

namespace AFI_Game.Controllers;
public class GameController : Controller
{
	private readonly AppDbContext _context;
	private readonly GameService _gameService;
	public GameController(AppDbContext context, GameService gameService)
	{
		_context = context;
		_gameService = gameService;
	}
	public IActionResult Lobby(int id)
	{
		var game = _gameService.GetGameByID(id);

		if (game == null)
		{
			return NotFound();
		}

		return View(game);
	}

	public IActionResult Play(int id)
	{
		var game = _gameService.GetGameByID(id);

		if (game == null)
		{
			return NotFound();
		}

		return View(game);
	}
}