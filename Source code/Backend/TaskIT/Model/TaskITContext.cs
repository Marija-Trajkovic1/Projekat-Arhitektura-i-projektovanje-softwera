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
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Jedan korisnik može postaviti više oglasa
            modelBuilder.Entity<JobAdvertisement>()
                .HasOne(o => o.MyEmployer)
                .WithMany(k => k.UserPostedAdv)
                .HasForeignKey(o => o.MyEmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasKey(ja => new { ja.JobId, ja.WorkerId });

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.JobAdvertisement)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Worker)
                .WithMany(u => u.UserAppliedAdv)
                .HasForeignKey(ja => ja.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.Employer)
                .WithMany()
                .HasForeignKey(p => p.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinishedJob>()
                .HasOne(p => p.JobAdvertisement)
                .WithMany()
                .HasForeignKey(p => p.JobAdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.Follower)
                .WithMany()
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.Followed)
                .WithMany()
                .HasForeignKey(uf => uf.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
