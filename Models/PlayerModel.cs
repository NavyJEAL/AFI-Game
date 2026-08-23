using System.ComponentModel.DataAnnotations.Schema;
namespace AFI_Game.Models;
public class Player
{
	public int PlayerID { get; set; }
	public int Points { get; set; }
	public int TurnOrder { get; set; }
	public int AccountID { get; set; }
	public int GameID { get; set; }
	[ForeignKey("AccountID")]
	public Account? Account { get; set; }
}