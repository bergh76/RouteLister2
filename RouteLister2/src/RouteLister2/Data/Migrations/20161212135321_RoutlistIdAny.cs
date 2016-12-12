using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RouteLister2.Data.Migrations
{
    public partial class RoutlistIdAny : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RouteLists_RouteListId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RouteListId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RouteLists_RouteListId",
                table: "Orders",
                column: "RouteListId",
                principalTable: "RouteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RouteLists_RouteListId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RouteListId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RouteLists_RouteListId",
                table: "Orders",
                column: "RouteListId",
                principalTable: "RouteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
