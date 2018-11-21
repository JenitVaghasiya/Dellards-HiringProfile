using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using svihire.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace svihire.Contexts
{
    public class AccountContext : DbContext
    {
        public string _accountID;

        public AccountContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<AccountContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public List<Account> Accounts
        {
            get
            {
                return _Accounts.SqlQuery("select * from dbo.Accounts where Id = '" + _accountID + "';").ToList();
            }
        }

        public DbSet<Account> _Accounts { get; set; }
    }

    public class InviteContext : DbContext
    {
        public InviteContext()
            : base("name=HiringProfile")
        {
            Database.SetInitializer<InviteContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invite>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public DbSet<Invite> _Invites { get; set; }

        public List<ExistingSurveyData> CheckIfCandidateHasCompletedByID(Guid surveyID, Guid candidateID)
        {
            List<ExistingSurveyData> existingData = new List<ExistingSurveyData>();
            try
            {
                existingData = Database.SqlQuery<ExistingSurveyData>("[dbo].[HIRE_TEST_CHECKCANDIDATEDATABYID] @p0, @p1", surveyID, candidateID).ToList();
            }
            catch (Exception ex)
            {
                var nothing = ex.StackTrace;
            }

            return existingData;
        }

    }

    public class QuestionContext : DbContext
    {
        public string _accountID;

        public QuestionContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<QuestionContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public List<Question> Questions
        {
            get
            {
                return _Questions.SqlQuery("select * from dbo.Questions where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public DbSet<Question> _Questions { get; set; }
    }

    public class QuestionResponseContext : DbContext
    {
        public string _accountID;

        public QuestionResponseContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<QuestionResponseContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionResponse>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public List<QuestionResponse> QuestionResponses
        {
            get
            {
                return _QuestionResponses.SqlQuery("select * from dbo.QuestionResponses where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public DbSet<QuestionResponse> _QuestionResponses { get; set; }


        public List<QuestionRanges> QuestionRanges
        {
            get
            {
                return _QuestionRanges.SqlQuery("select * from dbo.QuestionRenges where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public DbSet<QuestionRanges> _QuestionRanges { get; set; }
    }

    public class RespondentContext : DbContext
    {
        public string _accountID;

        public RespondentContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<RespondentContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Candidate>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public List<Candidate> Respondents
        {
            get
            {
                return _Respondents.SqlQuery("select * from dbo.Respondents where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public DbSet<Candidate> _Respondents { get; set; }
    }

    public class ResponseContext : DbContext
    {
        public string _accountID;

        public ResponseContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<ResponseContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Response>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public List<Response> Responses
        {
            get
            {
                return _Responses.SqlQuery("select * from dbo.Responses where AccountID = '" + _accountID + "';").ToList();
            }
        }
        public List<Response> ResponseByRecordId(Guid recordID)
        {
            return Database.SqlQuery<Response>("select * from dbo.Responses where RecordId = '" + recordID + "' and AccountID = '" + _accountID + "';").ToList();
        }

        public List<Guid> ResponseByCandidateIdSurveyId(string surveyId, string candidateId)
        {
            return Database.SqlQuery<Guid>("select distinct RecordId from dbo.Responses where SurveyId = '" + surveyId + "' and CandidateId='"+ candidateId + "' and AccountID = '" + _accountID + "';").ToList();
        }

        public DbSet<Response> _Responses { get; set; }
    }

    public class SurveyContext : DbContext
    {
        public Guid _accountID;

        public SurveyContext(Guid accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<SurveyContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public Candidate GetCandidateByInviteID(Guid inviteID)
        {
            return Database.SqlQuery<Candidate>("[dbo].[HIRE_TEST_GetCandidateByInviteId] @p0", inviteID).FirstOrDefault();
        }

        public List<SurveyItem> LoadSurveyItems(Guid surveyID)
        {
            return Database.SqlQuery<SurveyItem>("[dbo].[HIRE_TEST_LoadSurveyItems] @p0", surveyID).ToList();
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
       
        public List<Survey> Surveys
        {
            get
            {
                return _Surveys.SqlQuery("select * from dbo.Surveys where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public Candidate GetCandidateByID(Guid id)
        {
            return Database.SqlQuery<Candidate>("select * from dbo.Candidates where Id= '" + id + "';").FirstOrDefault();
        }


        public List<Candidate> Candidates
        {
            get
            {
                return Database.SqlQuery<Candidate>("select * from dbo.Candidates where AccountID = '" + _accountID + "';").ToList();
            }
        }
        public DbSet<QuestionRanges> QuestionRenges { get; set; }
        public DbSet<Survey> _Surveys { get; set; }

        public List<Categories> Categories
        {
            get
            {
                return Database.SqlQuery<Categories>("select * from dbo.Categories where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public List<CategoryQuestions> CategoryQuestions
        {
            get
            {
                return Database.SqlQuery<CategoryQuestions>("select * from dbo.CategoryQuestions where AccountID = '" + _accountID + "';").ToList();
            }
        }

    }

    public class DashboardContext : DbContext
    {
        public string _accountID;

        public DashboardContext(string accountID)
            : base("name=HiringProfile")
        {
            _accountID = accountID;
            Database.SetInitializer<DashboardContext>(null);
        }

        public HiringManagerIdentityModel GetUser(string userID)
        {
            return Database.SqlQuery<HiringManagerIdentityModel>("select * from dbo.aspnetusers where id = '" + userID + "';").FirstOrDefault();
        }

        public List<Survey> Surveys
        {
            get
            {
                return Database.SqlQuery<Survey>("select * from dbo.Surveys where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public List<HiringManagerIdentityModel> Managers
        {
            get
            {
                return Database.SqlQuery<HiringManagerIdentityModel>("select * from dbo.aspnetusers where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public List<Invite> Invites
        {
            get
            {
                return Database.SqlQuery<Invite>("select * from dbo.Invites where AccountID = '" + _accountID + "';").ToList();
            }
        }

        public List<InviteViewModel> GetAccountInvites()
        {
            var invites = new List<InviteViewModel>();
            //var cmd = Database.Connection.CreateCommand();
            //cmd.CommandText = "[dbo].[HIRE_TEST_GETACCOUNTINVITES]";
            try
            {
                invites = Database.SqlQuery<InviteViewModel>("[dbo].[HIRE_TEST_GETACCOUNTINVITES] @p0", _accountID).ToList();
            }
            catch(Exception ex)
            {
                var nothing = ex.StackTrace;
            }

            return invites;
            
        }
        
        public List<ExistingSurveyData> CheckIfCandidateHasCompletedByEmail(string surveyID, string email)
        {
            var existingCandidateData = new List<ExistingSurveyData>();

            try
            {
                existingCandidateData = Database.SqlQuery<ExistingSurveyData>("[dbo].[HIRE_TEST_CHECKIFCANDIDATEHASDATA] @p0, @p1", surveyID, email).ToList();
            }
            catch (Exception ex)
            {
                var nothing = ex.StackTrace;
            }

            return existingCandidateData;
        }
    }
}
