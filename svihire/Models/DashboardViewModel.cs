using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svihire.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel() { }

        public List<Survey> Positions { get; set; }
        public List<HiringManagerIdentityModel> Managers { get; set; }
        public List<InviteViewModel> Invites { get; set; }
        public List<Candidate> Candidates { get; set; }
        public HiringManagerIdentityModel User { get; set; }
        
    }
}