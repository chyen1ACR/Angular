using ChapterPortal.Controllers;
using ChapterPortal.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChapterPortal.DAL
{
    public class ChapterPortalDAL
    {
        private chapter_portalContext _context;

        public ChapterPortalDAL()
        {
            _context = new chapter_portalContext();
        }
     

        public IEnumerable<Announcement> GetAnnouncements(List<ChapterLookUp> obj)
        {
           var data = new List<Announcement>();
            var ids = string.Empty;
            foreach(ChapterLookUp item in obj)
            {
                var d = _context.Announcement.Where(a => a.ChpaterId == item.ChapterId);
                foreach(var n in d)
                {
                    data.Add((Announcement)n);
                }
            }
           return data;
        }
        public IEnumerable<Announcement> SaveAnnouncement(AnnouncementDto obj)
        {
            if (obj.ChpaterId.Count > 0)
            {
                foreach (var item in obj.ChpaterId)
                {
                    _context.Announcement.Add(new Announcement
                    {
                        PlainText = obj.PlainText,
                        Text = obj.Text,
                        ChpaterId = item.ChapterId,
                        CreatedBy = obj.CreatedBy,
                        StartDate = obj.StartDate,
                        EndDate = obj.EndDate,
                        ModifiedBy = obj.ModifiedBy,
                        ModifiedDate = new DateTime().ToString()
                    });
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Announcement.Add(new Announcement
                {
                    PlainText = obj.PlainText,
                    Text = obj.Text,
                    CreatedBy = obj.CreatedBy,
                    StartDate = obj.StartDate,
                    EndDate = obj.EndDate,
                    ModifiedBy = obj.ModifiedBy,
                    ModifiedDate = new DateTime().ToString()
                });
                _context.SaveChanges();
            }
            return _context.Announcement.ToList();
        }

        public void DeleteAnnouncement(Announcement obj)
        {
            _context.Announcement.Remove(_context.Announcement.FirstOrDefault(a => a.Id == obj.Id));
            _context.SaveChanges();
        }


        public void SaveOrUpdateChapters(IList<UserController.Chapter>  obj)
        {
            foreach(var item in obj)
            {
                ChapterLookUp data = _context.ChapterLookUp.Where(ch => ch.ChapterId == item.ChapterId).SingleOrDefault();
                    if (data == null) {
                    _context.ChapterLookUp.Add(new ChapterLookUp
                    {
                        ChapterId = item.ChapterId,
                        ChapterName = item.Name
                    });
                }
                else if(data.ChapterName != item.Name)
                {
                    data.ChapterName = item.Name;
                }
                _context.SaveChanges();
            }
        }

        public void SaveOrUpdateChapterportalOffice(ChapterOfficerDetails data)
        {
            _context.ChapterOfficerDetails.Add(data);
            _context.SaveChanges();
        }

        public IEnumerable<Folders> SaveOrUpdateFiles(int folderid,string FolderTitile, string filename, string filePath, string Type, bool IsFolderUpload  )
        {
            string filetype = (Type.ToLower() == "jpeg" || Type.ToLower() == "jpg") ? "jpeg" : Type.ToLower();
            Folders data = _context.Folders.Where(f => f.FolderId == folderid).SingleOrDefault();
			FolderTitile = FolderTitile == "" ? "New Folder" : FolderTitile;
            if(data == null)
            {
                var fol = new Folders
                {
                    FolderName = FolderTitile,
                    ParentId = 0,
                    Level = 0,
                    CreatedBy = "userone",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "userone",
                    ModifiedDate = DateTime.Now

                };

                _context.Folders.Add(fol);
                _context.SaveChanges();
                var fil = new Files
                {
                    Folder = fol,
                    FolderId = fol.FolderId,
                    FilePath = filePath,
                    FileType = 1,
                    CreatedBy = "userone",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "userone",
                    ModifiedDate = DateTime.Now,
                    Name = filename
                };
                _context.Files.Add(fil);
                _context.SaveChanges();
            }
            else if(folderid > 0 && IsFolderUpload)
            {
                var fol = new Folders
                {
                    FolderName = FolderTitile,
                    ParentId = folderid,
                    Level = 0,
                    CreatedBy = "userone",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "userone",
                    ModifiedDate = DateTime.Now

                };

                _context.Folders.Add(fol);
                _context.SaveChanges();
                var fil = new Files
                {
                    Folder = fol,
                    FolderId = fol.FolderId,
                    FilePath = filePath,
                    FileType = 1,
                    CreatedBy = "userone",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "userone",
                    ModifiedDate = DateTime.Now,
                    Name = filename
                };
                _context.Files.Add(fil);
                _context.SaveChanges();
            }
            else
            {
                var fil = new Files
                {
                    Folder = data,
                    FolderId = data.FolderId,
                    FilePath = filePath,
                    FileType = 1,
                    CreatedBy = "userone",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "userone",
                    ModifiedDate = DateTime.Now,
                    Name = filename
                };
                _context.Files.Add(fil);
                _context.SaveChanges();
            }


            return _context.Folders.Where(f => f.FolderId == folderid);

        }
        

    }
}
