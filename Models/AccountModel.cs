namespace AFI_Game.Models;
public class AccountModel
{
	public int AccountID { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
	public int Wins { get; set; }
}