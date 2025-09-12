using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextHireApp.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationInCvAndJobApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AbpUsers_Id",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_Companies_TaxCode",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_UserCode",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ViewerCode",
                table: "CvViews");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "AppJobApplications");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "UserCVs",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CvName",
                table: "UserCVs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "UserCVs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "UserCVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ViewedUserCode",
                table: "CvViews",
                type: "nvarchar(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ViewedCvId",
                table: "CvViews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "Companies",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TaxCode",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AppJobApplications",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JobCode",
                table: "AppJobApplications",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CvFile",
                table: "AppJobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Jobs_JobCode",
                table: "Jobs",
                column: "JobCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppJobApplications",
                table: "AppJobApplications",
                column: "ApplicationId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AppUser_UserCode",
                table: "AppUser",
                column: "UserCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AbpAuditLogExcelFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogExcelFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCVs_UserCode",
                table: "UserCVs",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_CvViews_ViewedCvId",
                table: "CvViews",
                column: "ViewedCvId");

            migrationBuilder.CreateIndex(
                name: "IX_CvViews_ViewedUserCode",
                table: "CvViews",
                column: "ViewedUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserCode",
                table: "Companies",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobApplications_JobCode",
                table: "AppJobApplications",
                column: "JobCode");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_User_Job",
                table: "AppJobApplications",
                columns: new[] { "UserCode", "JobCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_UserCode",
                table: "AppUser",
                column: "UserCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobApplications_AppUser_UserCode",
                table: "AppJobApplications",
                column: "UserCode",
                principalTable: "AppUser",
                principalColumn: "UserCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobApplications_Jobs_JobCode",
                table: "AppJobApplications",
                column: "JobCode",
                principalTable: "Jobs",
                principalColumn: "JobCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CvViews_AppUser_ViewedUserCode",
                table: "CvViews",
                column: "ViewedUserCode",
                principalTable: "AppUser",
                principalColumn: "UserCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CvViews_UserCVs_ViewedCvId",
                table: "CvViews",
                column: "ViewedCvId",
                principalTable: "UserCVs",
                principalColumn: "CvId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCVs_AppUser_UserCode",
                table: "UserCVs",
                column: "UserCode",
                principalTable: "AppUser",
                principalColumn: "UserCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobApplications_AppUser_UserCode",
                table: "AppJobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_AppJobApplications_Jobs_JobCode",
                table: "AppJobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_CvViews_AppUser_ViewedUserCode",
                table: "CvViews");

            migrationBuilder.DropForeignKey(
                name: "FK_CvViews_UserCVs_ViewedCvId",
                table: "CvViews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCVs_AppUser_UserCode",
                table: "UserCVs");

            migrationBuilder.DropTable(
                name: "AbpAuditLogExcelFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserCVs_UserCode",
                table: "UserCVs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Jobs_JobCode",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_CvViews_ViewedCvId",
                table: "CvViews");

            migrationBuilder.DropIndex(
                name: "IX_CvViews_ViewedUserCode",
                table: "CvViews");

            migrationBuilder.DropIndex(
                name: "IX_Companies_UserCode",
                table: "Companies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AppUser_UserCode",
                table: "AppUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_UserCode",
                table: "AppUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppJobApplications",
                table: "AppJobApplications");

            migrationBuilder.DropIndex(
                name: "IX_AppJobApplications_JobCode",
                table: "AppJobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_User_Job",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "UserCVs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "UserCVs");

            migrationBuilder.DropColumn(
                name: "ViewedCvId",
                table: "CvViews");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppJobApplications",
                newName: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "CvName",
                table: "UserCVs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ViewedUserCode",
                table: "CvViews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)");

            migrationBuilder.AddColumn<string>(
                name: "ViewerCode",
                table: "CvViews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "TaxCode",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "JobCode",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "CvFile",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxCode",
                table: "Companies",
                column: "TaxCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_UserCode",
                table: "AppUsers",
                column: "UserCode",
                unique: true,
                filter: "[UserCode] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AbpUsers_Id",
                table: "AppUsers",
                column: "Id",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
