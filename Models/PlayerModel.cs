namespace AFI_Game.Models;
public class Player
{
	public int PlayerID { get; set; }
	public int Points { get; set; }
	public int TurnOrder { get; set; }
	public int AccountID { get; set; }
	public int GameID { get; set; }
	public Account? Account { get; set; }
}