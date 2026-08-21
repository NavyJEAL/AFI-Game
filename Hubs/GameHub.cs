using Microsoft.AspNetCore.SignalR;

namespace AFI_Game.Hubs
{
	public class GameHub : Hub
	{
		public async Task JoinLobbyGroup(int gameId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
		}

		public async Task LeaveLobbyGroup(int gameId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"game-{gameId}");
		}

		public async Task JoinLobbyListGroup()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "lobby-list");
		}
	}
}