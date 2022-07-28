

using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Pizza> Pizza { get; set; }

        public DbSet<Ingrediente> Ingrediente { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Message> Messages { get; set; }
        public PizzaContext()
        {

        }

        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=pizze-db;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
