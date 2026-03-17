using MealLedger.Models;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace MealLedger.ApplicationDbContext
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<LunchRegistration> LunchRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UNIQUE constraint → one registration per employee per day
            modelBuilder.Entity<LunchRegistration>()
                .HasIndex(x => new { x.WorkdayID, x.LunchDate })
                .IsUnique();

            //// Role check constraint
            //modelBuilder.Entity<Employee>()
            //    .HasCheckConstraint("CK_Employee_Role", "Role IN ('Admin', 'Employee')");

            //// Holiday Type check constraint
            //modelBuilder.Entity<Holiday>()
            //    .HasCheckConstraint("CK_Holiday_Type", "Type IN ('Company', 'Optional')");
        }
    }
}
