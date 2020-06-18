using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class ChildFolders
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public int ChildFolderId { get; set; }

        public virtual Folders ChildFolder { get; set; }
        public virtual Folders Folder { get; set; }
    }
}
