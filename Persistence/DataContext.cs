using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> 
    {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }
        public DbSet<Activity> Activities { get; set; }


        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Comment> Comments { get; set; }


        public DbSet<UserFollowing> UserFollowings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

        
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);
            
            builder.Entity<Comment>()
                .HasOne(a => a.Activity)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            
               
            builder.Entity<UserFollowing>( b =>{
                b.HasKey(k => new { k.ObserverId, k.TargetId });

                b.HasOne( u => u.Observer)
                .WithMany( f => f.Followings)
                .HasForeignKey( f => f.ObserverId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne( u => u.Target)
                .WithMany( f => f.Followers)
                .HasForeignKey( f => f.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }

    }
        
}