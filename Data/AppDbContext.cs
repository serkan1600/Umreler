using Microsoft.EntityFrameworkCore;
using Umre.Web.Models;
using System;

namespace Umre.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<NewEntity> NewEntities { get; set; }

        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<UmreGuide> UmreGuides { get; set; }
        public DbSet<UmreHotel> UmreHotels { get; set; }
        public DbSet<UmreEducation> UmreEducations { get; set; }
        public DbSet<UmreForm> UmreForms { get; set; }
        public DbSet<HacRegistration> HacRegistrations { get; set; }
        public DbSet<UmreReservation> UmreReservations { get; set; }
        public DbSet<TourFlow> TourFlows { get; set; }
        public DbSet<Guide> Guides { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Popup> Popups { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<CallRequest> CallRequests { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Countdown> Countdowns { get; set; }
        public DbSet<TourCategory> TourCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SitePage> SitePages { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table mappings to Turkish names
            modelBuilder.Entity<AdminUser>().ToTable("Yoneticiler");
            modelBuilder.Entity<BlogPost>().ToTable("BlogYazilari");
            modelBuilder.Entity<Booking>().ToTable("Rezervasyonlar");
            modelBuilder.Entity<CallRequest>().ToTable("AramaTalepleri");
            modelBuilder.Entity<ContactMessage>().ToTable("IletisimMesajlari");
            modelBuilder.Entity<Countdown>().ToTable("GeriSayimlar");
            modelBuilder.Entity<Faq>().ToTable("SSS");
            modelBuilder.Entity<Feature>().ToTable("Ozellikler");
            modelBuilder.Entity<GalleryItem>().ToTable("GaleriOgeleri");
            modelBuilder.Entity<HacRegistration>().ToTable("HacKayitlari");
            modelBuilder.Entity<Menu>().ToTable("Menuler");
            modelBuilder.Entity<NewsletterSubscriber>().ToTable("BultenAboneleri");
            modelBuilder.Entity<Package>().ToTable("Paketler");
            modelBuilder.Entity<Popup>().ToTable("AcilirPencereler");
            modelBuilder.Entity<Service>().ToTable("Hizmetler");
            modelBuilder.Entity<SiteSetting>().ToTable("SiteAyarlari");
            modelBuilder.Entity<Slider>().ToTable("Slaytlar");
            modelBuilder.Entity<Testimonial>().ToTable("MusteriYorumlari");
            modelBuilder.Entity<TourCategory>().ToTable("TurKategorileri");
            modelBuilder.Entity<TourFlow>().ToTable("TurAkislari");
            modelBuilder.Entity<Guide>().ToTable("Rehberler");
            modelBuilder.Entity<UmreEducation>().ToTable("UmreEgitimleri");
            modelBuilder.Entity<UmreForm>().ToTable("UmreFormlari");
            modelBuilder.Entity<UmreGuide>().ToTable("UmreRehberleri");
            modelBuilder.Entity<UmreHotel>().ToTable("UmreOtelleri");
            modelBuilder.Entity<UmreReservation>().ToTable("UmreRezervasyonlari");
            // FK: UmreReservation -> Package (nullable)
            modelBuilder.Entity<UmreReservation>()
                .HasOne(u => u.Package)
                .WithMany()
                .HasForeignKey(u => u.PackageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser { Id = 1, Username = "admin", Password = "1234" }
            );

            modelBuilder.Entity<SiteSetting>().HasData(
                new SiteSetting { Id = 1, Key = "About_Title", Value = "Huzur Turizm Hakkında" },
                new SiteSetting { Id = 2, Key = "About_Description", Value = "20 yılı aşkın tecrübemizle..." },
                new SiteSetting { Id = 3, Key = "Contact_Address", Value = "Pirimehmet, 1710. Sk. No:43 iç kapı no 15, 32100 Merkez/Isparta" },
                new SiteSetting { Id = 4, Key = "Contact_Phone", Value = "0545 483 69 43" },
                new SiteSetting { Id = 5, Key = "Contact_Email", Value = "info@tefekkurturizm.com" },
                new SiteSetting { Id = 6, Key = "Contact_MapUrl", Value = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3010.518629088523!2d28.94770281571556!3d41.01389862705504!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14cab98d360d5b63%3A0x6295551949179!2sFatih%20Camii!5e0!3m2!1str!2str!4v1633023222099!5m2!1str!2str" },
                new SiteSetting { Id = 7, Key = "Exchange_Info", Value = "USD: 34.50 ₺  |  EUR: 37.20 ₺  |  SAR: 9.15 ₺" },
                new SiteSetting { Id = 100, Key = "Contact_Whatsapp", Value = "905454836943" }
            );

            modelBuilder.Entity<Package>().HasData(
                new Package
                {
                    Id = 1,
                    Title = "2026 Ekonomik Hac Turu",
                    Description = "Kutsal topraklarda manevi yolculuk.",
                    Price = 6000,
                    Currency = "USD",
                    StartDate = DateTime.Today.AddMonths(5),
                    EndDate = DateTime.Today.AddMonths(5).AddDays(15),
                    IsActive = true,
                    ImageUrl = "hac1.jpg"
                },
                new Package
                {
                    Id = 2,
                    Title = "2026 Lüks Hac Turu",
                    Description = "5 yıldız konforunda hac ibadeti.",
                    Price = 9000,
                    Currency = "USD",
                    StartDate = DateTime.Today.AddMonths(5),
                    EndDate = DateTime.Today.AddMonths(5).AddDays(20),
                    IsActive = true,
                    ImageUrl = "hac2.jpg"
                },
                new Package
                {
                    Id = 3,
                    Title = "Ramazan Umresi",
                    Description = "Ramazan ayının manevi atmosferinde umre.",
                    Price = 2500,
                    Currency = "USD",
                    StartDate = DateTime.Today.AddMonths(2),
                    EndDate = DateTime.Today.AddMonths(2).AddDays(14),
                    IsActive = true,
                    ImageUrl = "umre1.jpg"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
