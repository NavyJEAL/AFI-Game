using AFI_Game.Models;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<PlayerModel> Player { get; set; }
	public DbSet<PlayerCardModel> PlayerCard { get; set; }
	public DbSet<MovieCardModel> MovieCard { get; set; }
	public DbSet<GameModel> Game { get; set; }
	public DbSet<AccountModel> Account { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<PlayerModel>(entity =>
		{
			entity.ToTable("Player");
			entity.HasKey(e => e.PlayerID);
			entity.Property(e => e.PlayerID).HasColumnName("player_id");
			entity.Property(e => e.Points).HasColumnName("points");
			entity.Property(e => e.OrderNumber).HasColumnName("turn_order");
			entity.Property(e => e.AccountID).HasColumnName("account_id");
			entity.Property(e => e.GameID).HasColumnName("game_id");
		});

		modelBuilder.Entity<PlayerCardModel>(entity =>
		{
			entity.ToTable("PlayerCard");
			entity.HasKey(e => e.PlayerCardID);
			entity.Property(e => e.PlayerCardID).HasColumnName("playercard_id");
			entity.Property(e => e.Position).HasColumnName("position");
			entity.Property(e => e.CardID).HasColumnName("card_id");
			entity.Property(e => e.PlayerID).HasColumnName("player_id");
		});

		modelBuilder.Entity<MovieCardModel>(entity =>
		{
			entity.ToTable("MovieCard");
			entity.HasKey(e => e.CardID);
			entity.Property(e => e.CardID).HasColumnName("card_id");
			entity.Property(e => e.Title).HasColumnName("movie_title");
			entity.Property(e => e.Director).HasColumnName("director");
			entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
			entity.Property(e => e.Genre).HasColumnName("genre");
		});

		modelBuilder.Entity<GameModel>(entity =>
		{
			entity.ToTable("Game");
			entity.HasKey(e => e.GameID);
			entity.Property(e => e.GameID).HasColumnName("game_id");
			entity.Property(e => e.GameStatus).HasColumnName("game_status");
			entity.Property(e => e.CurrentPlayerID).HasColumnName("current_player");
			entity.Property(e => e.amountOfPlayers).HasColumnName("amount_players");
		});

		modelBuilder.Entity<AccountModel>(entity =>
		{
			entity.ToTable("Account");
			entity.HasKey(e => e.AccountID);
			entity.Property(e => e.AccountID).HasColumnName("account_id");
			entity.Property(e => e.Username).HasColumnName("account_name");
			entity.Property(e => e.Password).HasColumnName("password_hash");
			entity.Property(e => e.Wins).HasColumnName("wins");
		});
		
	}
}