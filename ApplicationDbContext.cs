using Microsoft.EntityFrameworkCore;
using InstructorInformationManagement.Models;

namespace InstructorInformationManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Instructor> Instructors { get; set; } = null!;
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}