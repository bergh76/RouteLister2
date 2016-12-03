using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RouteLister2.Data.Migrations
{
    public partial class movedvehicletoapplicationuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLists_Vehicles_VehicleId",
                table: "RouteLists");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_RouteLists_VehicleId",
                table: "RouteLists");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "RouteLists");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RouteLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersConnectionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersConnectionStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersConnectionStatus_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteLists_ApplicationUserId",
                table: "RouteLists",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnectionStatus_ApplicationUserId",
                table: "UsersConnectionStatus",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLists_AspNetUsers_ApplicationUserId",
                table: "RouteLists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLists_AspNetUsers_ApplicationUserId",
                table: "RouteLists");

            migrationBuilder.DropTable(
                name: "UsersConnectionStatus");

            migrationBuilder.DropIndex(
                name: "IX_RouteLists_ApplicationUserId",
                table: "RouteLists");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RouteLists");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "RouteLists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegistrationNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteLists_VehicleId",
                table: "RouteLists",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLists_Vehicles_VehicleId",
                table: "RouteLists",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
