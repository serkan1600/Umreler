using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminFormMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Menus (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                VALUES ('Başvuru Yönetimi', '#', 5, 1, 1, NULL, 'fas fa-file-signature');

                DECLARE @ParentId INT = (SELECT TOP 1 Id FROM Menus WHERE Title = 'Başvuru Yönetimi' AND IsAdmin = 1);

                INSERT INTO Menus (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                VALUES ('Hac Ön Kayıtlar', '/Admin/HacForms', 1, 1, 1, @ParentId, 'fas fa-kaaba');

                INSERT INTO Menus (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                VALUES ('Umre Kayıtlar', '/Admin/UmreReservations', 2, 1, 1, @ParentId, 'fas fa-mosque');

                INSERT INTO Menus (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                VALUES ('Anketler', '/Admin/UmreForms', 3, 1, 1, @ParentId, 'fas fa-poll');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
