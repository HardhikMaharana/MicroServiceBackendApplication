using Microsoft.EntityFrameworkCore;
using TeacherService.Models;

namespace TeacherService.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        public DbSet<Teachers> Teachers {  get; set; }  
    }
}
