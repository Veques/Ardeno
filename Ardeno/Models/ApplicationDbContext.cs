using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
