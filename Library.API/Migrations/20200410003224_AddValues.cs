using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.API.Migrations
{
    public partial class AddValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("f73f3072-a350-4e25-bbf7-a7823d90f55b"));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDate", "BirthPlace", "Email", "Name" },
                values: new object[] { new Guid("3503d92a-1ad9-4967-bc38-7dbab4ec82ea"), new DateTimeOffset(new DateTime(1993, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), "江苏·如东", "author@xxx.com", "Neil" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("3503d92a-1ad9-4967-bc38-7dbab4ec82ea"));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDate", "BirthPlace", "Email", "Name" },
                values: new object[] { new Guid("f73f3072-a350-4e25-bbf7-a7823d90f55b"), new DateTimeOffset(new DateTime(1993, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), "江苏·如东", "author@xxx.com", "Neil" });
        }
    }
}
