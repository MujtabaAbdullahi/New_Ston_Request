using Microsoft.EntityFrameworkCore;
using New_Ston_Request.Models;

namespace New_Ston_Request.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Category> Category { get; set; }
    }
}
