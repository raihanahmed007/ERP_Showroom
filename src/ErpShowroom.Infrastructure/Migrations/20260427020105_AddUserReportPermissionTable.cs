using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpShowroom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserReportPermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReportPermission",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ReportNameId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false),
                    CanPrint = table.Column<bool>(type: "bit", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReportPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReportPermission_ReportName_ReportNameId",
                        column: x => x.ReportNameId,
                        principalSchema: "erp_sys",
                        principalTable: "ReportName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReportPermission_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "erp_sys",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReportPermission_ReportNameId",
                schema: "erp_sys",
                table: "UserReportPermission",
                column: "ReportNameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReportPermission_UserId",
                schema: "erp_sys",
                table: "UserReportPermission",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReportPermission",
                schema: "erp_sys");
        }
    }
}
