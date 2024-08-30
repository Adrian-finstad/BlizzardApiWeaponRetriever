using Microsoft.EntityFrameworkCore;

namespace BlizzardApiWeaponRetriever
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }



        public DbSet<Weapon> Weapons { get; set; }


    }
}
