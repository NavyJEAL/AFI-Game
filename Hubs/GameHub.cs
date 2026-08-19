using Microsoft.AspNetCore.SignalR;

namespace AFI_Game.Hubs
{
	public class GameHub : Hub
	{
		public async Task JoinLobbyGroup(int gameId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
		}
	}
}