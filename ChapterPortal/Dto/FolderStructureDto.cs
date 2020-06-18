using ChapterPortal.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChapterPortal.Dto
{
    public class FolderStructureDto
    {
    }
    public class FileModelDto

    {

        public int FileID { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public int FolderID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }



    public class FolderModelDto

    {

        public int FolderID { get; set; }

        public string FolderName { get; set; }

        public int? ParentID { get; set; }

        public short Level { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }


        public List<FileModelDto> Files { get; set; }
        public List<FolderModelDto> ChildFolders { get; set; }

    }
}
