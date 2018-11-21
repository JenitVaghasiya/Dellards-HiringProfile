using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }        
    }
}
