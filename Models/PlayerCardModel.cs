namespace AFI_Game.Models;
public class PlayerCard
{
	public int PlayerCardID { get; set; }
	public int Position { get; set; }
	public int CardID { get; set; }
	public int PlayerID { get; set; }

	public MovieCard MovieCard { get; set; }
    public Player Player { get; set; }
}