using Microsoft.AspNetCore.SignalR;

namespace AFI_Game.Hubs
{
	public class GameHub : Hub
	{
		// You can add methods here that clients can call via SignalR
		public async Task JoinLobbyGroup(int gameId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
		}
	}
}