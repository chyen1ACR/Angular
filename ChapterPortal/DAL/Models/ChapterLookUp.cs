using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class ChapterLookUp
    {
        public int Id { get; set; }
        public string ChapterId { get; set; }
        public string ChapterName { get; set; }
    }
}
