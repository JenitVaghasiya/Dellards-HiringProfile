using System;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class CategoryQuestions
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
