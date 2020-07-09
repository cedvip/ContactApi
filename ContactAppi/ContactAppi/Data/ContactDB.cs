using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactAppi.Models
{
    public class ContactDB
    {
        [Key]
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<ContactSkill> ContactSkills { get; set; }
    }
}
