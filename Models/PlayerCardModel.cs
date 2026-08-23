using System.ComponentModel.DataAnnotations.Schema;
namespace AFI_Game.Models;
public class PlayerCard
{
	public int PlayerCardID { get; set; }
	public int Position { get; set; }
	public int CardID { get; set; }
	public int PlayerID { get; set; }
	[ForeignKey("CardID")]
	public MovieCard MovieCard { get; set; }
    [ForeignKey("PlayerID")]
    public Player Player { get; set; }
}