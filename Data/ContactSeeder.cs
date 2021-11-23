using ContactApp.Data.Models;
using ContactApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactApp.Data
{
    public class ContactSeeder
    {
        private readonly ContactContext _ctx;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<User> _userManager;

        public ContactSeeder(ContactContext ctx,
                             IWebHostEnvironment hosting,
                             UserManager<User> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            User user = await _userManager.FindByEmailAsync("contactdemo@test.com");

            if (user == null)
            {
                user = new User()
                {
                    FirstName = "Contact",
                    LastName = "Demo",
                    Email = "contactdemo@test.com",
                    UserName = "contactdemo@test.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in Seeder");
                }
            }

            if (!_ctx.Contacts.Any())
            {
                // Need to create the Sample Data
                var file = Path.Combine(_hosting.ContentRootPath, "Data/contact.json");
                var json = File.ReadAllText(file);
                var contacts = JsonSerializer.Deserialize<IEnumerable<Contact>>(json);
                _ctx.Contacts.AddRange(contacts);

                var contactgrp = _ctx.ContactGroups.Where(o => o.ContactGroupId == 1).FirstOrDefault();
                if (contactgrp != null)
                {
                    contactgrp.User = user;
                    contactgrp.ContactItems = new List<Contact>();
                }
                _ctx.SaveChanges();
            }
        }
    }
}
