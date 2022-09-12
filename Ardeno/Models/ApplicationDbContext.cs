using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Ardeno.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContext)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Database\Ardeno.db");
            dbContext.UseSqlite($"Data Source = {path}");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}
