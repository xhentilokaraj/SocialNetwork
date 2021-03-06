﻿using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Data
{
    public class SocialNetworkContext : DbContext
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestion { get; set; }
        public DbSet<SocialNetwork.Models.Country> Country { get; set; }
        public DbSet<SocialNetwork.Models.City> City { get; set; }
        public DbSet<SocialNetwork.Models.UserRelationship> UserRelationship { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Email)
                .IsUnique();
        }


        public DbSet<SocialNetwork.Models.Community> Community { get; set; }


        public DbSet<SocialNetwork.Models.CommunityMember> CommunityMember { get; set; }
    }
}