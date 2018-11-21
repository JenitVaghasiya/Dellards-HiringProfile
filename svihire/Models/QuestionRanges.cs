using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class QuestionRanges: BaseEntity
    {
        public Guid AccountId { get; set; }
        public Guid QuestionId { get; set; }
        public decimal RangeMin { get; set; }

        public decimal RangeMax { get; set; }
    }
}
