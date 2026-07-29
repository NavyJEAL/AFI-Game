using AFI_Game.Models;

namespace AFI_Game.ViewModels;
public class GameLobbyViewModel
{
	public required int GameID { get; set; }
	public required string GameStatus { get; set; }
	public required int CurrentPlayerID { get; set; }
	public required List<Player> Players { get; set; } = new List<Player>();

}