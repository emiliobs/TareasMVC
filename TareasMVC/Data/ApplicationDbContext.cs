using Microsoft.EntityFrameworkCore;

namespace TareasMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

    }
}
