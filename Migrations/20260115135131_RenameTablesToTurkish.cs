using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToTurkish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_PackageId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_TourCategories_CategoryId",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreReservations",
                table: "UmreReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreHotels",
                table: "UmreHotels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreGuides",
                table: "UmreGuides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreForms",
                table: "UmreForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreEducations",
                table: "UmreEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourCategories",
                table: "TourCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Testimonials",
                table: "Testimonials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteSettings",
                table: "SiteSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Popups",
                table: "Popups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsletterSubscribers",
                table: "NewsletterSubscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HacRegistrations",
                table: "HacRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GalleryItems",
                table: "GalleryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features",
                table: "Features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Faqs",
                table: "Faqs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countdowns",
                table: "Countdowns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CallRequests",
                table: "CallRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUsers",
                table: "AdminUsers");

            migrationBuilder.RenameTable(
                name: "UmreReservations",
                newName: "UmreRezervasyonlari");

            migrationBuilder.RenameTable(
                name: "UmreHotels",
                newName: "UmreOtelleri");

            migrationBuilder.RenameTable(
                name: "UmreGuides",
                newName: "UmreRehberleri");

            migrationBuilder.RenameTable(
                name: "UmreForms",
                newName: "UmreFormlari");

            migrationBuilder.RenameTable(
                name: "UmreEducations",
                newName: "UmreEgitimleri");

            migrationBuilder.RenameTable(
                name: "TourCategories",
                newName: "TurKategorileri");

            migrationBuilder.RenameTable(
                name: "Testimonials",
                newName: "MusteriYorumlari");

            migrationBuilder.RenameTable(
                name: "Sliders",
                newName: "Slaytlar");

            migrationBuilder.RenameTable(
                name: "SiteSettings",
                newName: "SiteAyarlari");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Hizmetler");

            migrationBuilder.RenameTable(
                name: "Popups",
                newName: "AcilirPencereler");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Paketler");

            migrationBuilder.RenameTable(
                name: "NewsletterSubscribers",
                newName: "BultenAboneleri");

            migrationBuilder.RenameTable(
                name: "Menus",
                newName: "Menuler");

            migrationBuilder.RenameTable(
                name: "HacRegistrations",
                newName: "HacKayitlari");

            migrationBuilder.RenameTable(
                name: "GalleryItems",
                newName: "GaleriOgeleri");

            migrationBuilder.RenameTable(
                name: "Features",
                newName: "Ozellikler");

            migrationBuilder.RenameTable(
                name: "Faqs",
                newName: "SSS");

            migrationBuilder.RenameTable(
                name: "Countdowns",
                newName: "GeriSayimlar");

            migrationBuilder.RenameTable(
                name: "ContactMessages",
                newName: "IletisimMesajlari");

            migrationBuilder.RenameTable(
                name: "CallRequests",
                newName: "AramaTalepleri");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Rezervasyonlar");

            migrationBuilder.RenameTable(
                name: "BlogPosts",
                newName: "BlogYazilari");

            migrationBuilder.RenameTable(
                name: "AdminUsers",
                newName: "Yoneticiler");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_CategoryId",
                table: "Paketler",
                newName: "IX_Paketler_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PackageId",
                table: "Rezervasyonlar",
                newName: "IX_Rezervasyonlar_PackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreRezervasyonlari",
                table: "UmreRezervasyonlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreOtelleri",
                table: "UmreOtelleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreRehberleri",
                table: "UmreRehberleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreFormlari",
                table: "UmreFormlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreEgitimleri",
                table: "UmreEgitimleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TurKategorileri",
                table: "TurKategorileri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusteriYorumlari",
                table: "MusteriYorumlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slaytlar",
                table: "Slaytlar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteAyarlari",
                table: "SiteAyarlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hizmetler",
                table: "Hizmetler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcilirPencereler",
                table: "AcilirPencereler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paketler",
                table: "Paketler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BultenAboneleri",
                table: "BultenAboneleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menuler",
                table: "Menuler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HacKayitlari",
                table: "HacKayitlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GaleriOgeleri",
                table: "GaleriOgeleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ozellikler",
                table: "Ozellikler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SSS",
                table: "SSS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeriSayimlar",
                table: "GeriSayimlar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IletisimMesajlari",
                table: "IletisimMesajlari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AramaTalepleri",
                table: "AramaTalepleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rezervasyonlar",
                table: "Rezervasyonlar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogYazilari",
                table: "BlogYazilari",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yoneticiler",
                table: "Yoneticiler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Paketler_TurKategorileri_CategoryId",
                table: "Paketler",
                column: "CategoryId",
                principalTable: "TurKategorileri",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervasyonlar_Paketler_PackageId",
                table: "Rezervasyonlar",
                column: "PackageId",
                principalTable: "Paketler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paketler_TurKategorileri_CategoryId",
                table: "Paketler");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervasyonlar_Paketler_PackageId",
                table: "Rezervasyonlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yoneticiler",
                table: "Yoneticiler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreRezervasyonlari",
                table: "UmreRezervasyonlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreRehberleri",
                table: "UmreRehberleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreOtelleri",
                table: "UmreOtelleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreFormlari",
                table: "UmreFormlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmreEgitimleri",
                table: "UmreEgitimleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TurKategorileri",
                table: "TurKategorileri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SSS",
                table: "SSS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slaytlar",
                table: "Slaytlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteAyarlari",
                table: "SiteAyarlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rezervasyonlar",
                table: "Rezervasyonlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paketler",
                table: "Paketler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ozellikler",
                table: "Ozellikler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusteriYorumlari",
                table: "MusteriYorumlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menuler",
                table: "Menuler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IletisimMesajlari",
                table: "IletisimMesajlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hizmetler",
                table: "Hizmetler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HacKayitlari",
                table: "HacKayitlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeriSayimlar",
                table: "GeriSayimlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GaleriOgeleri",
                table: "GaleriOgeleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BultenAboneleri",
                table: "BultenAboneleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogYazilari",
                table: "BlogYazilari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AramaTalepleri",
                table: "AramaTalepleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcilirPencereler",
                table: "AcilirPencereler");

            migrationBuilder.RenameTable(
                name: "Yoneticiler",
                newName: "AdminUsers");

            migrationBuilder.RenameTable(
                name: "UmreRezervasyonlari",
                newName: "UmreReservations");

            migrationBuilder.RenameTable(
                name: "UmreRehberleri",
                newName: "UmreGuides");

            migrationBuilder.RenameTable(
                name: "UmreOtelleri",
                newName: "UmreHotels");

            migrationBuilder.RenameTable(
                name: "UmreFormlari",
                newName: "UmreForms");

            migrationBuilder.RenameTable(
                name: "UmreEgitimleri",
                newName: "UmreEducations");

            migrationBuilder.RenameTable(
                name: "TurKategorileri",
                newName: "TourCategories");

            migrationBuilder.RenameTable(
                name: "SSS",
                newName: "Faqs");

            migrationBuilder.RenameTable(
                name: "Slaytlar",
                newName: "Sliders");

            migrationBuilder.RenameTable(
                name: "SiteAyarlari",
                newName: "SiteSettings");

            migrationBuilder.RenameTable(
                name: "Rezervasyonlar",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "Paketler",
                newName: "Packages");

            migrationBuilder.RenameTable(
                name: "Ozellikler",
                newName: "Features");

            migrationBuilder.RenameTable(
                name: "MusteriYorumlari",
                newName: "Testimonials");

            migrationBuilder.RenameTable(
                name: "Menuler",
                newName: "Menus");

            migrationBuilder.RenameTable(
                name: "IletisimMesajlari",
                newName: "ContactMessages");

            migrationBuilder.RenameTable(
                name: "Hizmetler",
                newName: "Services");

            migrationBuilder.RenameTable(
                name: "HacKayitlari",
                newName: "HacRegistrations");

            migrationBuilder.RenameTable(
                name: "GeriSayimlar",
                newName: "Countdowns");

            migrationBuilder.RenameTable(
                name: "GaleriOgeleri",
                newName: "GalleryItems");

            migrationBuilder.RenameTable(
                name: "BultenAboneleri",
                newName: "NewsletterSubscribers");

            migrationBuilder.RenameTable(
                name: "BlogYazilari",
                newName: "BlogPosts");

            migrationBuilder.RenameTable(
                name: "AramaTalepleri",
                newName: "CallRequests");

            migrationBuilder.RenameTable(
                name: "AcilirPencereler",
                newName: "Popups");

            migrationBuilder.RenameIndex(
                name: "IX_Rezervasyonlar_PackageId",
                table: "Bookings",
                newName: "IX_Bookings_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Paketler_CategoryId",
                table: "Packages",
                newName: "IX_Packages_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUsers",
                table: "AdminUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreReservations",
                table: "UmreReservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreGuides",
                table: "UmreGuides",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreHotels",
                table: "UmreHotels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreForms",
                table: "UmreForms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmreEducations",
                table: "UmreEducations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourCategories",
                table: "TourCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faqs",
                table: "Faqs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteSettings",
                table: "SiteSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features",
                table: "Features",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Testimonials",
                table: "Testimonials",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                table: "Menus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HacRegistrations",
                table: "HacRegistrations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countdowns",
                table: "Countdowns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GalleryItems",
                table: "GalleryItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsletterSubscribers",
                table: "NewsletterSubscribers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CallRequests",
                table: "CallRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Popups",
                table: "Popups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Packages_PackageId",
                table: "Bookings",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_TourCategories_CategoryId",
                table: "Packages",
                column: "CategoryId",
                principalTable: "TourCategories",
                principalColumn: "Id");
        }
    }
}
