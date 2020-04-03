using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Events.Data.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserCategoryTickets> UserCategoryTickets { get; set; }
        public DbSet<UserEventSignUp> UserEventSignUps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Config
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<UserCategoryTickets>().HasKey(c => new { c.UserId, c.CategoryId });
            builder.Entity<UserCategoryTickets>().HasOne(o => o.Category).WithMany(o => o.UserTickets).HasForeignKey(o => o.CategoryId);
            builder.Entity<UserCategoryTickets>().HasOne(o => o.User).WithMany(o => o.CategoryTickets).HasForeignKey(o => o.UserId);

            builder.Entity<UserEventSignUp>().HasKey(o => new { o.EventId, o.UserId });
            builder.Entity<UserEventSignUp>().HasOne(o => o.Event).WithMany(o => o.UserSignUps).HasForeignKey(o => o.EventId);
            builder.Entity<UserEventSignUp>().HasOne(o => o.User).WithMany(o => o.EventSignUps).HasForeignKey(o => o.UserId);

            // Seed
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Badminton",
                },
                new Category
                {
                    Id = 2,
                    Name = "Football",
                },
                new Category
                {
                    Id = 3,
                    Name = "Social",
                },
                new Category
                {
                    Id = 4,
                    Name = "Video games",
                },
                new Category
                {
                    Id = 5,
                    Name = "Board games",
                }
            );

            // TODO investigate BST, Offsets
            var today = DateTime.Today;
            var nextWednesday = today.AddDays(((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7);
            var nextWednesdayAtSix = nextWednesday.AddHours(18);

            var fourEvents = Enumerable.Range(0, 4)
                .Select(o => new Event
                {
                    Id = o + 1,
                    Capacity = 8,
                    CategoryId = 1,
                    Date = nextWednesdayAtSix.AddDays(7 * o),
                    Location = "Stratford Leisure Centre"
                });

            builder.Entity<Event>().HasData(fourEvents);
        }
    }
}
