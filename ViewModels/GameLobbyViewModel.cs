using AFI_Game.Models;

namespace AFI_Game.ViewModels;
public class GameLobbyViewModel
{
	public required int GameID { get; set; }
	public required string GameStatus { get; set; }
	public required int CurrentPlayerID { get; set; }
	public required List<PlayerModel> Players { get; set; } = new List<PlayerModel>();
	public required int amountOfPlayers { get; set; }
}