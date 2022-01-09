using System;
using InternetForum.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetForum.Infrastructure.Repository
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //one to one
            modelBuilder.Entity<User>().HasOne(k => k.UserDetails).WithOne(m => m.User).HasForeignKey<UserDetails>(n => n.User_Id);

            //one to many
            modelBuilder.Entity<User>().HasMany(k => k.Posts).WithOne(m => m.Author);

            //many to one
            modelBuilder.Entity<Post>().HasOne(k => k.Author).WithMany(m => m.Posts);
        }

        public DbSet<Reply> Reply { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        
        
       
    }
}
