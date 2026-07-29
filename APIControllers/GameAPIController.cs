using Microsoft.EntityFrameworkCore;
using AFI_Game.Models;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.ViewModels;
using AFI_Game.Services;

[ApiController]
[Route("api/[controller]")]
public class GameAPIController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly GameService _gameService;
	public GameAPIController(AppDbContext context, GameService gameService)
	{
		_context = context;
		_gameService = gameService;
	}

	//Get the current game status
	//PlayerCard stuff like place card
	//MovieCard stuff like get movie card

}