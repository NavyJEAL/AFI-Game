namespace AFI_Game.Models;
public class GameModel
{
	public int GameID { get; set; }
	public string? GameStatus { get; set; }
	public int CurrentPlayerID { get; set; }
	public List<PlayerModel> Players { get; set; } = new List<PlayerModel>();
	public int amountOfPlayers { get; set; }
}