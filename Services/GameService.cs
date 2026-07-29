using AFI_Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AFI_Game.Services;
public class GameService
{
	private readonly AppDbContext _context;
	
	public GameService(AppDbContext context)
	{
		_context = context;
	}

	public Game? GetGameByID(int gameID)
	{
		return _context.Game.Include(g => g.Players).FirstOrDefault(g => g.GameID == gameID);
	}
	
	public Game CreateGame()
	{
		var game = new Game {GameStatus = "Waiting"};
		_context.Game.Add(game);
		_context.SaveChanges();
		return game;
	}

	public Player JoinGame(int gameID, int accountID)
	{
		var game = GetGameByID(gameID);

		if(game == null)
		{
			throw new Exception("Game not found");
		}

		if(game.GameStatus != "Waiting")
		{
			throw new Exception("Game has already started");
		}

		var existingPlayer = game.Players.Any(p => p.AccountID == accountID);
		if (existingPlayer)
		{
			throw new Exception("Has already joined a game");
		}

		var player = new Player{ 
			AccountID = accountID,
    		GameID = gameID,
    		Points = 0,
    		TurnOrder = game.Players.Count + 1
		};

		_context.Player.Add(player);
		_context.SaveChanges();
		return player;
	}

	public Game StartGame(int gameID)
	{
		var game = GetGameByID(gameID);
		if(game == null)
		{
			throw new Exception("Game not found");
		}

		if(game.Players.Count < 2)
		{
			throw new Exception("Not enough players");
		}
		
		game.GameStatus = "Active";
		var firstPlayer = game.Players.OrderBy(p => p.TurnOrder).First();
		game.CurrentPlayerID = firstPlayer.PlayerID;
		_context.SaveChanges();
		
		return game;
	}

	public Player GetNextPlayer()
	{
		
	}

	public Game AdvanceTurn(int gameID)
	{
		var game = GetGameByID(gameID);
		if(game == null)
		{
			throw new Exception("Game not found");
		}

		var currentPlayer = game.Players.FirstOrDefault(p => p.PlayerID == game.CurrentPlayerID);
		if(currentPlayer == null)
		{
			throw new Exception("Current player not found");
		}

		var orderedPlayers = game.Players.OrderBy(p => p.TurnOrder).ToList();
		// t.ex. 4 spelare (0,1,2,3), om index är 3 blir det (3+1) % 4 = 4 % 4 = 0
		var currentIndex = orderedPlayers.FindIndex(p => p.PlayerID == currentPlayer.PlayerID);
		var nextIndex = (currentIndex + 1) % orderedPlayers.Count;

		game.CurrentPlayerID = orderedPlayers[nextIndex].PlayerID;

		_context.SaveChanges();

		return game;
	}

	public PlayerCard PlaceCard()
	{
		
	}

	public int GetPlayerPoints(int gameID, int playerID)
	{
		var game = GetGameByID(gameID);
		if(game == null)
		{
			throw new Exception("Game not found");
		}

		var player = game.Players.FirstOrDefault(p => p.PlayerID == playerID);
		if(player == null)
		{
			throw new Exception("Player not found");
		}

		var playerPoints = player.Points;

		if(playerPoints == 10)
		{
			EndGame(game.GameID, player.PlayerID);
		}
		return playerPoints;
	}

	public Game EndGame(int gameID, int winningPlayerID)
	{
		var game = GetGameByID(gameID);
		if(game == null)
		{
			throw new Exception("Game not found");
		}

		game.GameStatus = "Ended";

		var winner = game.Players.FirstOrDefault(p => p.PlayerID == winningPlayerID);
		if(winner != null)
		{
			var account = _context.Account.FirstOrDefault(a => a.AccountID == winner.AccountID);
			if(account != null)
			{
				account.Wins =+ 1;
			}
		}

		_context.SaveChanges();
		
		return game;
	}
}