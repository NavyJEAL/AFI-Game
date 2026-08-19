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
	/*
	public List<PlayerCard> GetPlayerCard()
	{
		
	}

	public PlayerCard GetPlayerTimeline(int playerID)
	{
		
	}*/

}