using AFI_Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFI_Game.Services;
public class MovieCardService
{
	private readonly AppDbContext _context;
	private readonly Random _random = new Random();
	public MovieCardService(AppDbContext context)
	{
		_context = context;
	}

	public MovieCard GetRandomUnusedCard(int gameID)
	{
		var usedCardIDs = _context.PlayerCard
			.Where(pc => pc.Player.GameID == gameID)
			.Select(pc => pc.CardID)
			.ToList();

		var avaibleCards = _context.MovieCard
			.Where(mc => !usedCardIDs.Contains(mc.CardID))
			.ToList();
		
		if(avaibleCards.Count == 0)
		{
			throw new Exception("No more available cards");
		}

		var randomIndex = _random.Next(avaibleCards.Count);
		return avaibleCards[randomIndex];
	}

	
}