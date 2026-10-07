using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBD.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAttractionDetailsAndAttractionFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttractionId",
                table: "Feedbacks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanceFromTownKm",
                table: "Attractions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImagesJson",
                table: "Attractions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HowToReach",
                table: "Attractions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Attractions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Attractions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravelTimeMinutes",
                table: "Attractions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AttractionId",
                table: "Feedbacks",
                column: "AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Attractions_AttractionId",
                table: "Feedbacks",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Attractions_AttractionId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_AttractionId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DistanceFromTownKm",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "GalleryImagesJson",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "HowToReach",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "TravelTimeMinutes",
                table: "Attractions");
        }
    }
}
