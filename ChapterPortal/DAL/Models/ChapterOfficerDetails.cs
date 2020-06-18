using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class ChapterOfficerDetails
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string PositionDescription { get; set; }
        public string PositionBeginDate { get; set; }
        public string PositionEndDate { get; set; }
        public string VotingStatus { get; set; }
        public string PrimaryEmail { get; set; }
        public string PrimaryPhone { get; set; }
        public string AdditionalComments { get; set; }
    }
}
