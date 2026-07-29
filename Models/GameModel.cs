namespace AFI_Game.Models;
public class Game
{
	public int GameID { get; set; }
	public string? GameStatus { get; set; }
	public int CurrentPlayerID { get; set; }
	public List<Player> Players { get; set; } = new List<Player>();
}