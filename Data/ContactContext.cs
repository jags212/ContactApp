using ContactApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ContactApp.Models;

namespace ContactApp.Data
{
    public class ContactContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _config;
        public ContactContext(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactGroup> ContactGroups { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder bldr)
        {
            base.OnConfiguring(bldr);

            bldr.UseSqlServer(_config.GetConnectionString("ContactDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContactGroup>()
            .HasData(new ContactGroup()
            {
                ContactGroupId = 1,
                Name = "12345"
            });
        }
    }
}
