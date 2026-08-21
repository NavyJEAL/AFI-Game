using AFI_Game.Models;

namespace AFI_Game.ViewModels;
public class HomePageViewModel
{
	public required string Username { get; set; }
	public required int AccountID { get; set; }
	public List<Game> Games { get; set; } = new();

}