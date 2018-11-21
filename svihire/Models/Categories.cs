using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Categories : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string CategoryName { get; set; }
    }
}
