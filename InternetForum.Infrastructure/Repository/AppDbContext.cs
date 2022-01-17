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

            //one to many - post
            modelBuilder.Entity<User>().HasMany(k => k.Posts).WithOne(m => m.Author);

            //many to one - post
            modelBuilder.Entity<Post>().HasOne(k => k.Author).WithMany(m => m.Posts);

            //many to one - post
            modelBuilder.Entity<Post>().HasMany(k => k.Replies).WithOne(m => m.Post).OnDelete(DeleteBehavior.Cascade); ;

            //one to many - reply
            modelBuilder.Entity<User>().HasMany(k => k.Replies).WithOne(m => m.Author);

            //many to one - reply
            modelBuilder.Entity<Reply>().HasOne(k => k.Author).WithMany(m => m.Replies);


        }



        public DbSet<Reply> Reply { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        
        
       
    }
}
