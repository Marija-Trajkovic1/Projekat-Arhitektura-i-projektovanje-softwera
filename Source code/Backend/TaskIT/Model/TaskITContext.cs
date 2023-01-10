using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TaskIT.Model
{
    public class TaskITContext : DbContext
    {
        public TaskITContext(DbContextOptions op) : base(op)
        {
        }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Radnik> Radnici { get; set; }
        public DbSet<Poslodavac> Poslodavci { get; set; }
        public DbSet<OglasZaPosao> OglasiZaPosao { get; set; }
        public DbSet<TipPosla> TipoviPoslova { get; set; }
    }
}
