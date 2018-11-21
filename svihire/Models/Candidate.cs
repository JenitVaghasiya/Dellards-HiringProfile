using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Candidate : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        
        
    }
}
