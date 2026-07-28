namespace AFI_Game.Models;
public class MovieCardModel
{
	public int CardID { get; set; }
	public required string Title { get; set; }
	public required string Director { get; set; }
	public int ReleaseYear { get; set; }
	public string? Genre { get; set; }
}	