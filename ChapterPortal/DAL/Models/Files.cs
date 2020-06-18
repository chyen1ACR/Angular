using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class Files
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public short? FileType { get; set; }
        public int FolderId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Folders Folder { get; set; }
    }
}
