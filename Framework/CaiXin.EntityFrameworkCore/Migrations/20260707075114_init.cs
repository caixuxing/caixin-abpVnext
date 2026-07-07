using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiXin.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false, comment: "主键ID"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "用户名称"),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "用户密码"),
                    Salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "盐值"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "创建者"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "更新者"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleter = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "删除者"),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                },
                comment: "用户信息表");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
