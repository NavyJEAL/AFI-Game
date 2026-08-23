using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AFI_Game.Migrations
{
    /// <inheritdoc />
    public partial class FixPlayerCardForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_name = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    wins = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_status = table.Column<string>(type: "text", nullable: true),
                    current_player = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.game_id);
                });

            migrationBuilder.CreateTable(
                name: "moviecard",
                columns: table => new
                {
                    card_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movie_title = table.Column<string>(type: "text", nullable: false),
                    director = table.Column<string>(type: "text", nullable: false),
                    release_year = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moviecard", x => x.card_id);
                });

            migrationBuilder.CreateTable(
                name: "player",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    points = table.Column<int>(type: "integer", nullable: false),
                    turn_order = table.Column<int>(type: "integer", nullable: false),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_player_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_game_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playercard",
                columns: table => new
                {
                    playercard_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    position = table.Column<int>(type: "integer", nullable: false),
                    card_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playercard", x => x.playercard_id);
                    table.ForeignKey(
                        name: "FK_playercard_moviecard_card_id",
                        column: x => x.card_id,
                        principalTable: "moviecard",
                        principalColumn: "card_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playercard_player_player_id",
                        column: x => x.player_id,
                        principalTable: "player",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_account_id",
                table: "player",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_game_id",
                table: "player",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_playercard_card_id",
                table: "playercard",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_playercard_player_id",
                table: "playercard",
                column: "player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "playercard");

            migrationBuilder.DropTable(
                name: "moviecard");

            migrationBuilder.DropTable(
                name: "player");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "game");
        }
    }
}
