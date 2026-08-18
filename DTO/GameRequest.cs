namespace AFI_Game.DTO
{
	public class JoinGameRequest
	{
		public int AccountID { get; set; }
	}

	public class EndGameRequest
	{
		public int WinningPlayerID { get; set; }
	}
}