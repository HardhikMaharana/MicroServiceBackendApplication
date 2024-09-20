using Microsoft.EntityFrameworkCore;

namespace AdminSubjectsAndClassesService.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optons) : base(optons) { }
    }
}
