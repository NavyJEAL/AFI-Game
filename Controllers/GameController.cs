using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.Models;
using AFI_Game.ViewModels;

namespace AFI_Game.Controllers;
public class GameController : Controller
{
	private readonly AppDbContext _context;
	public GameController(AppDbContext context)
	{
		_context = context;
	}

		[HttpGet]
	// Returns the EnterGame view
	public IActionResult JoinGame()
	{
		return View();
	}

	[HttpGet]
	public IActionResult CreateGame()
	{
		return View();
	}

	[HttpPost]
	// Returns the CreateGame viewmodel
	public IActionResult CreateGame(GameLobbyViewModel vm)
	{
		if(!ModelState.IsValid)
		{
			ModelState.AddModelError(string.Empty, "Invalid game creation attempt");
		}

		var existingGame = _context.Game.FirstOrDefault(g => g.CurrentPlayerID == vm.CurrentPlayerID);
		if(existingGame != null)
		{
			ModelState.AddModelError(string.Empty, "A game with this player already exists");
			return View(vm);
		}

		//måste kontrollera om en spelare redan finns i ett annat spel, samt hur många spelare som finns i spelet. Om det är 4 spelare i spelet, kan man inte lägga till fler spelare.
		var newGame = new GameModel
		{
			amountOfPlayers = 1,
			CurrentPlayerID = vm.CurrentPlayerID,
			GameStatus = "Waiting for players"
		};

		_context.Game.Add(newGame);
		_context.SaveChanges();

		return View(vm);
	}

	public IActionResult StartPage()
	{
		var allGames = _context.Game.ToList();
		return View(allGames);
	}
}