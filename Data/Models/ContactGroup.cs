using ContactApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactApp.Models
{
    public class ContactGroup
    {
        public long ContactGroupId { get; set; }
        public string Name { get; set; }
        public ICollection<Contact> ContactItems { get; set; }
        public User User { get; set; }
    }
}
