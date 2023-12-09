using MagicVilla.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Data
{
    public class ApplicationDbContext : DbContext
    {

        //injeccion de dependencia para aplicar el DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "First Villa",
                    Detail = "first villa created",
                    Price = 550,
                    ImageUrl = "",
                    Capacity = 30,
                    Province = "Cordoba",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },

            new Villa()
            {
                Id = 2,
                Name = "Second Villa",
                Detail = "second villa created",
                Price = 350,
                ImageUrl = "",
                Capacity = 60,
                Province = "Mendoza",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            }
                );
        }
    }
}

