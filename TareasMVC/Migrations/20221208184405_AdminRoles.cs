using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRoles : Migration
    {
        //roles roles
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT Exists(Select Id From AspNetRoles where Id = '1d5a17a7-5cad-4222-a0d5-5a5a8899cea9')
                                    Begin
	                                    Insert AspNetRoles (Id, Name, NormalizedName)
                                        Values ('1d5a17a7-5cad-4222-a0d5-5a5a8899cea9', 'admin','ADMIN')
                                    End
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Delete AspNetRoles Where Id = '1d5a17a7-5cad-4222-a0d5-5a5a8899cea9'");
        }
    }
}
