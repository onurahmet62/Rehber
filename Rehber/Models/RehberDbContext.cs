using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rehber.Models;
using System.Data.Common;

namespace Rehber.Models
{
    public class RehberDbContext : IdentityDbContext
    {
        public RehberDbContext(DbContextOptions<RehberDbContext> options) : base(options)
        {
        }
       
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }
    }
}
