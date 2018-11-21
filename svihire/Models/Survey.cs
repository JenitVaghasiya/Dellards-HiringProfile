using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Survey : BaseEntity
    {
        [Required]
        public string SurveyName { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }

    public class SurveyItem
    {
        public SurveyItem() { }

        public Guid AccountID { get; set; }
        public Guid SurveyID { get; set; }
        public string SurveyName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionOrder { get; set; }
        public Guid QuestionResponseId { get; set; }
        public string ResponseText { get; set; }
        public string ResponseScore { get; set; }

    }

    //public class TakeSurveyViewModel
    //{
    //    public TakeSurveyViewModel() { }

    //    public Candidate candidate { get; set; }
    //    public List<SurveyItem> surveyItems { get; set; }

    //}

    public class SurveyQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        
        public List<SurveyResponseViewModel> PossibleResponses { get; set; }
    }

    public class SurveyResponseViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int ResponseScore { get; set; }
    }

    public class TakeSurveyViewModel
    {
        public Guid SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public Candidate Candidate { get; set; }
        public Guid InviteId { get; set; }

        public List<SurveyQuestionViewModel> QuestionsViewModel { get; set; }
    }
}
