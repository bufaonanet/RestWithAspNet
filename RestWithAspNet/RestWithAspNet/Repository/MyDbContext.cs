using Microsoft.EntityFrameworkCore;
using RestWithAspNet.Model;

namespace RestWithAspNet.Data
{
    public class MyDbContext : DbContext
    {        
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
