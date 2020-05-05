using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class PluralizeDbSetNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bug_Projects_ProjectId",
                table: "Bug");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Bug_BugId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bug",
                table: "Bug");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Bug",
                newName: "Bugs");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BugId",
                table: "Comments",
                newName: "IX_Comments_BugId");

            migrationBuilder.RenameIndex(
                name: "IX_Bug_ProjectId",
                table: "Bugs",
                newName: "IX_Bugs_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bugs",
                table: "Bugs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_Projects_ProjectId",
                table: "Bugs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_Projects_ProjectId",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bugs_BugId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bugs",
                table: "Bugs");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Bugs",
                newName: "Bug");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BugId",
                table: "Comment",
                newName: "IX_Comment_BugId");

            migrationBuilder.RenameIndex(
                name: "IX_Bugs_ProjectId",
                table: "Bug",
                newName: "IX_Bug_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bug",
                table: "Bug",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bug_Projects_ProjectId",
                table: "Bug",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Bug_BugId",
                table: "Comment",
                column: "BugId",
                principalTable: "Bug",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
