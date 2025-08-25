using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskIT.Model
{
    public class TaskITContext : IdentityDbContext<User>

    {
        public TaskITContext(DbContextOptions<TaskITContext> op) : base(op)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<JobAdvertisement> JobAdvertisements { get; set; }
        public DbSet<FinishedJob> FinishedJobs { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }

        public DbSet<WorkerJobTypeFollowing> WorkerJobTypeFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Jedan korisnik može postaviti više oglasa
            modelBuilder.Entity<JobAdvertisement>()
                .HasOne(o => o.MyEmployer)
                .WithMany(k => k.UserPostedAdv)
                .HasForeignKey(o => o.MyEmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Jedan korisnik može se prijaviti na više oglasa
            modelBuilder.Entity<JobAdvertisement>()
                .HasOne(o => o.MyWorker)
                .WithMany(k => k.UserAppliedAdv)
                .HasForeignKey(o => o.MyWorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veza OdradjeniPosao -> Radnik
            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veza OdradjeniPosao -> Poslodavac
            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.Employer)
                .WithMany()
                .HasForeignKey(p => p.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veza OdradjeniPosao -> Oglas
            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.JobAdvertisement)
                .WithMany()
                .HasForeignKey(p => p.JobAdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
