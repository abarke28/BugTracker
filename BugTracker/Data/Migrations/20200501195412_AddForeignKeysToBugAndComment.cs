using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class AddForeignKeysToBugAndComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bug_Projects_ProjectId",
                table: "Bug");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Bug_BugId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "BugId",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bug",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bug_Projects_ProjectId",
                table: "Bug");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Bug_BugId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "BugId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Bug",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Bug_Projects_ProjectId",
                table: "Bug",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Bug_BugId",
                table: "Comment",
                column: "BugId",
                principalTable: "Bug",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
