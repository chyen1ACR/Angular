using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class Announcement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PlainText { get; set; }
        public string ChpaterId { get; set; }
        public int CreatedBy { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
