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

	public class CreateGameRequest
	{
		public int AccountID { get; set; }
	}

	public class StartGameRequest
	{
		public int GameID { get; set; }
	}

	public class LeaveGameRequest
	{
		public int AccountID { get; set; }
	}
	
}