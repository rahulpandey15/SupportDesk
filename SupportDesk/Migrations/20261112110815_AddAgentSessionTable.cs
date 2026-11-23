using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentSessions",
                columns: table => new
                {
                    SessionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConversationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StateJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentSessions", x => x.SessionKey);
                    table.ForeignKey(
                        name: "FK_AgentSessions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentSessions_CustomerId_AgentName_ConversationId",
                table: "AgentSessions",
                columns: new[] { "CustomerId", "AgentName", "ConversationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentSessions");
        }
    }
}
