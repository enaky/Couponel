﻿using Couponel.Entities.Coupons;
using Couponel.Entities.Identities;
using Couponel.Entities.Institutions;
using Couponel.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Couponel.Persistence
{
    public class CouponelContext : DbContext
    {
        public CouponelContext(DbContextOptions<CouponelContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CouponEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FacultyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RedeemedCouponEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UniversityEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<RedeemedCoupon> RedeemedCoupons { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
