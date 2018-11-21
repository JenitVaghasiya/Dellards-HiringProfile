using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Question : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid ResponseSetId { get; set; }
        public Guid SurveyId { get; set; }

        public Survey Survey { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public int Order { get; set; }        

        
    }
}
