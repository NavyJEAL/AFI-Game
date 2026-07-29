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

}