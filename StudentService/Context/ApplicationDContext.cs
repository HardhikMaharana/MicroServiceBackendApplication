using Microsoft.EntityFrameworkCore;

namespace StudentService.Context
{
    public class ApplicationDContext:DbContext
    {
        public ApplicationDContext(DbContextOptions<ApplicationDContext> options):base(options) { }
    }
}
