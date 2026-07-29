using Microsoft.AspNetCore.Mvc;
using AFI_Game.Models;
using AFI_Game.ViewModels;

namespace AFI_Game.Controllers;

public class AccountController : Controller
{
	private readonly AppDbContext _context;
	public AccountController(AppDbContext context)
	{
		_context = context;
	}
	
	// Returns the Register view
	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Register(RegisterViewModel vm)
	{
		if(!ModelState.IsValid)
		{
			ModelState.AddModelError(string.Empty, "Unable to register");
			return View(vm);
		}

		var existingAccount = _context.Account.FirstOrDefault(a => a.Username == vm.Username);
		if(existingAccount != null)
		{
			ModelState.AddModelError(string.Empty, "Username already exists");
			return View(vm);
		}

		var account = new AccountModel
		{
			Username = vm.Username,
			Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
			Wins = 0
		};

		_context.Account.Add(account);
		_context.SaveChanges();
		
		return View("HomePage", "Home");
		
	}

	// Returns the Login view
	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Login(LoginViewModel vm)
	{
		if(!ModelState.IsValid)
		{
			return View(vm);
		}

		var account = _context.Account.FirstOrDefault(a => a.Username == vm.Username);
		if(account == null || !BCrypt.Net.BCrypt.Verify(vm.Password, account.Password))
		{
			ModelState.AddModelError(string.Empty, "Invalid username or password");
			return View(vm);
		}

		return RedirectToAction("HomePage", "Home");
	}

	// Returns the Logout view
	public IActionResult Logout()
	{
		return View("Login");
	}

}
