using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GalleryMap.Database
{
    public class GalleryMapDbContextFactory : IDesignTimeDbContextFactory<GalleryMapDbContext>
    {
        public GalleryMapDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GalleryMapDbContext>();
            
            var tempDbPath = Path.Combine(Path.GetTempPath(), "design_time_maui.db");
            optionsBuilder.UseSqlite($"Data Source={tempDbPath}");

            return new GalleryMapDbContext(optionsBuilder.Options);
        }
    }
}
