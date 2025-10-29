using jwtAuthentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace jwtAuthentication.Data
{
    public class MyDBContext:DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
