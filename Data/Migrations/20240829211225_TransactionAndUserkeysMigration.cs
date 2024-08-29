using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UmbraChallenge.Migrations
{
    /// <inheritdoc />
    public partial class TransactionAndUserkeysMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTransferKeys",
                columns: table => new
                {
                    KeyId = table.Column<string>(type: "TEXT", nullable: false),
                    KeyType = table.Column<int>(type: "INTEGER", nullable: false),
                    KeyValue = table.Column<string>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTransferKeys", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_UserTransferKeys_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    SenderKeyId = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverKeyId = table.Column<string>(type: "TEXT", nullable: false),
                    TransferAmmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_UserTransferKeys_ReceiverKeyId",
                        column: x => x.ReceiverKeyId,
                        principalTable: "UserTransferKeys",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_UserTransferKeys_SenderKeyId",
                        column: x => x.SenderKeyId,
                        principalTable: "UserTransferKeys",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ApplicationUserId",
                table: "Transactions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverKeyId",
                table: "Transactions",
                column: "ReceiverKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderKeyId",
                table: "Transactions",
                column: "SenderKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransferKeys_ApplicationUserId",
                table: "UserTransferKeys",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserTransferKeys");
        }
    }
}
