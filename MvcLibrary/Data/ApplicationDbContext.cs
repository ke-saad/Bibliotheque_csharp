using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcLibrary.Models;

namespace MvcLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext<Adherent, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to be called first

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                // Configure the Book entity further if needed
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.ReservationId);

                entity.HasOne(d => d.Book)
                      .WithMany(p => p.Reservations)
                      .HasForeignKey(d => d.BookId)
                      .HasConstraintName("FK_Reservations_Books");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.Reservations)
                      .HasForeignKey(d => d.UserId) // This assumes you have a UserId in Reservation to link to IdentityUser
                      .HasConstraintName("FK_Reservations_Adherents");
            });

            // Additional model configurations can be added here
        }
    }
}
