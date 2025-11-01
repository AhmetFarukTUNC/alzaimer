
using AlzaimerApp.Data.Models;
using AlzheimerApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AlzheimerApp.Data
{
    public class AlzheimerContext : DbContext
    {
        public AlzheimerContext(DbContextOptions<AlzheimerContext> options)
            : base(options)
        {
        }

        public DbSet<Prediction> Predictions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

    }
}
