using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
{
    public class NewsletterContext : DbContext
    {
        public NewsletterContext(DbContextOptions<NewsletterContext> options)
           : base(options)
        {
        }
        public DbSet<Newsletter> Newsletter { get; set; } = null!;
    }
}

