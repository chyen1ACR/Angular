using System;
using System.Collections.Generic;

namespace ChapterPortal.DAL.Models
{
    public partial class Folders
    {
        public Folders()
        {
            ChildFoldersChildFolder = new HashSet<ChildFolders>();
            ChildFoldersFolder = new HashSet<ChildFolders>();
            Files = new HashSet<Files>();
        }

        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int? ParentId { get; set; }
        public short Level { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ChildFolders> ChildFoldersChildFolder { get; set; }
        public virtual ICollection<ChildFolders> ChildFoldersFolder { get; set; }
        public virtual ICollection<Files> Files { get; set; }
    }
}
