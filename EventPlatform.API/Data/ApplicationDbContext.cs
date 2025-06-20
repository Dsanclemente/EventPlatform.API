using Microsoft.EntityFrameworkCore;
using EventPlatform.API.Models;

namespace EventPlatform.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración del modelo Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Datos de ejemplo
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Conferencia de Tecnología 2024",
                    DateTime = DateTime.Now.AddDays(7),
                    Location = "Centro de Convenciones",
                    Description = "Una conferencia sobre las últimas tendencias en tecnología",
                    Status = EventStatus.Upcoming,
                    CreatedAt = DateTime.UtcNow
                },
                new Event
                {
                    Id = 2,
                    Title = "Meetup de Desarrolladores",
                    DateTime = DateTime.Now.AddDays(14),
                    Location = "Café Central",
                    Description = "Networking y charlas sobre desarrollo de software",
                    Status = EventStatus.Attending,
                    CreatedAt = DateTime.UtcNow
                },
                new Event
                {
                    Id = 3,
                    Title = "Workshop de Angular",
                    DateTime = DateTime.Now.AddDays(21),
                    Location = "Universidad Local",
                    Description = "Taller práctico sobre Angular 17+",
                    Status = EventStatus.Maybe,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 