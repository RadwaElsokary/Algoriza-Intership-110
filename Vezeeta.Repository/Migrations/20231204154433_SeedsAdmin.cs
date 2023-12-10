using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta.Repository.Migrations
{
    public partial class SeedsAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecialistId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0",
                column: "ConcurrencyStamp",
                value: "53baa627-daf5-4063-8683-cfba864954f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "0009b36c-acc9-4089-9bfc-a7a1b9cd4393");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "84805c16-e3fd-45d1-8eb0-41fcf1adcea6");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthOfDate", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhotoPath", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f24f5477-3489-4fe2-aa0a-e3c5fcd29a5f", 0, new DateTime(2001, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "a5e0e0a9-667b-4529-ace5-ec5caf353aaf", "ApplicationUser", "admin@email.com", false, "Admin", 1, "User", true, null, "ADMIN@EMAIL.COM", "ADMIN@EMAIL.COM", "AQAAAAEAACcQAAAAEHTXmCZqNJmKlhvLSzTPXF6IEJVGjwIRXYqH7vsCt7fVQSmKqpsbs3MpJCnAcnS8qg==", "0112913842", false, null, "cafe5282-2057-441a-b0c1-f01570966f9d", false, "admin@email.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0", "f24f5477-3489-4fe2-aa0a-e3c5fcd29a5f" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecialistId",
                table: "AspNetUsers",
                column: "SpecialistId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecialistId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0", "f24f5477-3489-4fe2-aa0a-e3c5fcd29a5f" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f24f5477-3489-4fe2-aa0a-e3c5fcd29a5f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0",
                column: "ConcurrencyStamp",
                value: "68af5328-22f5-49e5-a93d-264ee23c950f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e024e6cf-a2d6-46fb-94d4-4a49641d4ad3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a322e1a6-d1e8-44c8-a913-385540defdd6");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecialistId",
                table: "AspNetUsers",
                column: "SpecialistId",
                principalTable: "Specializations",
                principalColumn: "Id");
        }
    }
}
