using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniReserve.Infrastructure.Migrations
{
    public partial class AddUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.Sql(
                """
                ALTER TABLE "Users"
                ALTER COLUMN "Role" TYPE character varying(50)
                USING (
                    CASE "Role"
                        WHEN 0 THEN 'Client'
                        WHEN 1 THEN 'Admin'
                        ELSE 'Client'
                    END
                );
                """);

         
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Client",
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(
                """
                ALTER TABLE "Users"
                ALTER COLUMN "Role" TYPE integer
                USING (
                    CASE "Role"
                        WHEN 'Client' THEN 0
                        WHEN 'Admin' THEN 1
                        ELSE 0
                    END
                );
                """);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Client");
        }
    }
}