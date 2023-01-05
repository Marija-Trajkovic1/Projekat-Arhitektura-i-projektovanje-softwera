using Microsoft.EntityFrameworkCore;
using TaskITBackend.Models;

namespace TaskItBackend.Models
{
    public class TaskItContext : DbContext
    {
        public TaskItContext(DbContextOptions op):base(op)
        {
            
        }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Radnik> Radnici { get; set; }
        public DbSet<Poslodavac> Poslodavci { get; set; }
        public DbSet<OglasZaPosao> OglasiZaPosao { get; set; }
        public DbSet<TipPosla> TipoviPoslova { get; set; }
    }
}