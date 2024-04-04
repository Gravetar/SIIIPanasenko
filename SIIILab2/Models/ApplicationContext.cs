using Microsoft.EntityFrameworkCore;

namespace SIIILab2.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Traffic> Traffics { get; set; }
        public DbSet<Road>? Roads { get; set; }
        public DbSet<RequestRoad>? RequestRoads { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options = null) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Traffic traffic1 = new Traffic
            {
                Id = 1,
                Density = 1,
                PathFile = "\\Traffics\\1.xml"
            };
            Road road1 = new Road
            {
                Id = 1,
                Address = "г. Волгоград пр. Ленина 1",
                PathFile = "\\Roads\\1.xml",
                CoorLatitude1 = 48.7194,
                CoorLongitude1 = 44.5018,
                CoorLatitude2 = 49.7194,
                CoorLongitude2 = 49.5018,

                trafficid = 1,
            };
            RequestRoad rd1 = new RequestRoad
            {
                Id = 1,
                Costumer = "ООО ДОРОГА",
                Reques_date = new DateTime(2023, 11, 12, 17, 54, 11),
                Status = "В работе",
                Result = "NONE",

                roadid = 1
            };

            modelBuilder.Entity<Traffic>().HasData(traffic1);
            modelBuilder.Entity<Road>().HasData(road1);
            modelBuilder.Entity<RequestRoad>().HasData(rd1);
        }
    }
}
