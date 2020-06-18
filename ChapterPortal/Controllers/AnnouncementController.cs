using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ChapterPortal.DAL;
using ChapterPortal.DAL.Models;

namespace ChapterPortal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private ChapterPortalDAL _chapterportalDAL;
        public AnnouncementController()
        {
             _chapterportalDAL = new ChapterPortalDAL();
        }

        [Route("GetAnnoumncements")]
        [HttpPost]
        public IEnumerable<Announcement> GetAnnoumncements(List<ChapterLookUp> obj)
        {

            return _chapterportalDAL.GetAnnouncements(obj);
        }

        [HttpPost]
        public IEnumerable<Announcement> SaveAnnouncementes(AnnouncementDto obj)
        {           

            return _chapterportalDAL.SaveAnnouncement(obj);
        }

        //[Route("DeleteAnnouncement")]
        //[HttpPost]
        //public IEnumerable<Announcement> DeleteAnnouncement(Announcement obj)
        //{


        //    return _chapterportalDAL.GetAnnouncements(obj);
        //}
    }

    public class AnnouncementDto
    {
        public string Text { get; set; }
        public string PlainText { get; set; }
        public IList<ChapterLookUp> ChpaterId { get; set; }
        public int CreatedBy { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
