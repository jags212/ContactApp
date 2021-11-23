using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ContactApp.Models
{
    public class Contact
    {
        public long ContactId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public static implicit operator Task<object>(Contact v)
        {
            throw new NotImplementedException();
        }
    }
}
