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
		return _context.Game
			.Include(g => g.Players)
			.ThenInclude(p => p.Account)
			.FirstOrDefault(g => g.GameID == gameID);
	}
	
	public Player CreateGame(int accountID)
	{
		var game = new Game {GameStatus = "Waiting"};
		_context.Game.Add(game);
		_context.SaveChanges();

		var player = new Player
		{
			AccountID = accountID,
			GameID = game.GameID,
			Points = 0,
			TurnOrder = 1
		};
		_context.Player.Add(player);
		_context.SaveChanges();

		_context.Entry(player)
			.Reference(p => p.Account)
			.Load();
		
		return player;
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

		var player = new Player
		{
			AccountID = accountID,
    		GameID = gameID,
    		Points = 0,
    		TurnOrder = game.Players.Count + 1
		};

		_context.Player.Add(player);
		_context.SaveChanges();
		_context.Entry(player)
			.Reference(p => p.Account)
			.Load();

		return player;
	}

	public void LeaveGame(int gameID, int accountID)
	{
		var game = GetGameByID(gameID);
		if(game == null)
		{
			throw new Exception("Game not found");
		}

		var player = game.Players.FirstOrDefault(p => p.AccountID == accountID);
		if(player == null)
		{
			throw new Exception("Player not found in this game");
		}
		_context.Player.Remove(player);
		_context.SaveChanges();

		var remainingPlayers = _context.Player.Count(p => p.GameID == gameID);
		if(remainingPlayers == 0)
		{
			_context.Game.Remove(game);
			_context.SaveChanges();
		}else
		{
			var orderedPlayers = game.Players.OrderBy(p => p.TurnOrder).ToList();
			for(int i = 0; i < orderedPlayers.Count; i++)
			{
				orderedPlayers[i].TurnOrder = i + 1;
			}
			_context.SaveChanges();
		}
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

	public bool PlaceCard(int playerID, int cardID, int position)
	{
		var player = _context.Player.Find(playerID);
		if(player == null)
		{
			throw new Exception("Player not found");
		}

		// hämtar tidslinjen från databasen.
		var timeline = _context.PlayerCard
			.Include(pc => pc.MovieCard)
			.Where(pc => pc.PlayerID == playerID)
			.OrderBy(pc => pc.Position)
			.ToList();

		var newCard = _context.MovieCard.Find(cardID);
		if(newCard == null)
		{
			throw new Exception("Card not found");
		}
		// left = äldre release year, right nyare release year.
		var leftCard = timeline.FirstOrDefault(pc => pc.Position == position-1);
		var rightCard = timeline.FirstOrDefault(pc => pc.Position == position+1);

		var isCorrect = 
		(leftCard == null || leftCard.MovieCard.ReleaseYear < newCard.ReleaseYear) && 
		(rightCard == null || rightCard.MovieCard.ReleaseYear > newCard.ReleaseYear);

		if (isCorrect)
		{
			// flyttar fram samtliga kort ett steg åt höger från där det nya kortet placeras.
			foreach (var card in timeline.Where(pc => pc.Position >= position))
			{
				card.Position += 1;
			}

			var playerCard = new PlayerCard
			{
				PlayerID = playerID,
				Position = position,
				CardID = cardID
			};

			_context.PlayerCard.Add(playerCard);
			_context.SaveChanges();

			// Räknar spelarens kort och kontrollerar om det är 10st.
			var cardCount = _context.PlayerCard.Count(p => p.PlayerID == playerID);
			if(cardCount == 10)
			{
				EndGame(player.GameID, playerID);
			} 
		}
		return isCorrect;

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
				account.Wins += 1;
			}
		}

		_context.SaveChanges();
		
		return game;
	}
}