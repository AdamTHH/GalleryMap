using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleryMap.Models;
using Microsoft.EntityFrameworkCore;

namespace GalleryMap.Database
{
    public class GalleryMapDbContext : DbContext
    {
        public GalleryMapDbContext(DbContextOptions<GalleryMapDbContext> options)
            : base(options)
        {
        }

        public GalleryMapDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "maui.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        public DbSet<ImageLocation> ImageLocations { get; set; }
    }
}
