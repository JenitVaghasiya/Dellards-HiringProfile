using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Account : BaseEntity
    {
        [Required]
        public string CompanyName { get; set; }
        public string DomainName { get; set; }
    }
}
