using Microsoft.EntityFrameworkCore;
using AFI_Game.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GameAPIController : ControllerBase
{
	private readonly AppDbContext _context;
	public GameAPIController(AppDbContext context)
	{
		_context = context;
	}

	//Get the current game status
	//PlayerCard stuff like place card
	//MovieCard stuff like get movie card
}