using AFI_Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFI_Game.Services;

public class PlayerService
{
	private readonly AppDbContext _context;
	public PlayerService(AppDbContext context)
	{
		_context = context;
	}

	public Player? GetPlayerByID(int playerID)
	{
		return _context.Player.Include(p => p.Account).FirstOrDefault(p => p.PlayerID == playerID);
	}

	    public Player? GetPlayerByAccount(int gameId, int accountId)
    {
        return _context.Player
            .Include(p => p.Account)
            .FirstOrDefault(p => p.GameID == gameId && p.AccountID == accountId);
    }

    public List<PlayerCard> GetPlayerTimeline(int playerID)
    {
        return _context.PlayerCard
            .Include(pc => pc.MovieCard)
            .Where(pc => pc.PlayerID == playerID)
            .OrderBy(pc => pc.Position)
            .ToList();
    }

}