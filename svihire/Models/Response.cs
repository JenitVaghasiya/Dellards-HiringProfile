using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace svihire.Models
{
    public class Response : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Guid SurveyId { get; set; }
        public Guid CategoryId { get; set; }

        //public Account Account { get; set; }
        public Guid QuestionId { get; set; }
        public Guid QuestionResponseId { get; set; }

        public Guid CandidateId { get; set; }
        public Guid RecordId { get; set; }

        //public Candidate Respondent { get; set; }
        public string QuestionText { get; set; }
        public int ResponseScore { get; set; }
        // public string SurveyResponseText { get; set; }
    }

    public class SurveyReportViewModel
    {
        public Guid SurveyId { get; set; }
        public Guid recordId { get; set; }
        public string SurveyTitle { get; set; }
        public Candidate Candidate { get; set; }
        public decimal percentMatch { get; set; }
        public DateTimeOffset DateOfCompletion { get; set; }
        public List<ResponseCategoryViewModel> categories { get; set; }
        public List<ResponseQuestionViewModel> QuestionsViewModel { get; set; }

    }


    public class ResponseQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }

        public QuestionRangesViewModel QuestionRange { get; set; }

        public List<SurveyResponseViewModel> PossibleResponses { get; set; }
        public Response QuestionResponse { get; set; }
    }


    public class ResponseCategoryViewModel : Categories

    {

        public List<ResponseCategoryQuestionsViewModel> CategoryQuestions { get; set; }

        public int CountWithInRange { get; set; }


    }
    public class ResponseCategoryQuestionsViewModel : CategoryQuestions
    {
        public bool isInRange { get; set; }
    }
    public class QuestionRangesViewModel
    {
        public Guid AccountId { get; set; }
        public Guid QuestionId { get; set; }
        public decimal RangeMin { get; set; }
        public decimal RangeMax { get; set; }
    }
}
