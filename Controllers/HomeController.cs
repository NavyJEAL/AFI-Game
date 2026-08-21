using Microsoft.AspNetCore.Mvc;
using AFI_Game.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

	[Authorize]
	public IActionResult HomePage()
	{
		var username = User.Identity?.Name;
		var allGames = _context.Game.Include(g => g.Players).ToList();

		var viewModel = new HomePageViewModel
		{
			Username = username,
			AccountID = _context.Account.FirstOrDefault(a => a.Username == username)?.AccountID ?? 0,
			Games = allGames
		};
		return View(viewModel);
	}
}
