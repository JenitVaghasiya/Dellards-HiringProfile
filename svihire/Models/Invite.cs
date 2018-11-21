using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace svihire.Models
{
    public class Invite : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        //ManagerId is a string b/c it references aspnetusers table
        public string ManagerId { get; set; }

        public Guid SurveyId { get; set; }
        public Guid CandidateId { get; set; }
        public string Opening { get; set; }
    }

    public class InviteViewModel
    {
        public InviteViewModel() { }

        public Guid CandidateId { get; set; }
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public string CandidateEmail { get; set; }

        public string ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }

        public Guid SurveyId { get; set; }
        public string SurveyName { get; set; }

        public Guid InviteId { get; set; }
        public string Opening { get; set; }
    }

    public class ExistingSurveyData : InviteViewModel
    {
        public DateTimeOffset SurveyCompletionDate { get; set; }
    }

    public class ExistingSurveyDataViewModel
    {
        public ExistingSurveyDataViewModel() { }

        public List<ExistingSurveyData> existingData {get;set;}
        public Candidate candidate { get; set; }



    }
}
