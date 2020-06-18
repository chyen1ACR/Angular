using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChapterPortal.DAL;
using ChapterPortal.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChapterPortal.Controllers
{
    [Route("[controller]")]
    [ApiController]
        
    public class ChapterController : ControllerBase
    {

        private readonly IConfiguration Configuration;
        private readonly ChapterPortalDAL _chapterportalDAL;
        public ChapterController(IConfiguration configuration)
        {
            Configuration = configuration;
            _chapterportalDAL = new ChapterPortalDAL();

        }

        [Route("SaveOrUpdateChapteOfficer")]
        [HttpPost]
        public bool SaveOrUpdateChapteOfficer(ChapterOfficerDetails obj)
        {
            
                _chapterportalDAL.SaveOrUpdateChapterportalOffice(obj);
                return true;
            
           
        }
        
    }
}
