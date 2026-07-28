using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.Models;

[ApiController]
[Route("api/[controller]")]
public class PlayerAPIController : ControllerBase
{
	private readonly AppDbContext _context;
	public PlayerAPIController(AppDbContext context)
	{
		_context = context;
	}

	//Get timeline for a specific player
}