using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AFI_Game.Models;

namespace AFI_Game.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

	public IActionResult HomePage()
	{
		var allGames = _context.Game.ToList();
		return View(allGames);
	}
}
