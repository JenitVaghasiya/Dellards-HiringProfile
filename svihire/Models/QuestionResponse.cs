using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class QuestionResponse : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid ResponseSetId { get; set; }

        [Required]
        public string ResponseText { get; set; }        

        [Required]
        public int Order { get; set; }

        [Required]
        public int ResponseScore { get; set; }        
    }
}
