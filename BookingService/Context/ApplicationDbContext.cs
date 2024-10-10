using Microsoft.EntityFrameworkCore;

namespace BookingService.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options) { }
    }
}
