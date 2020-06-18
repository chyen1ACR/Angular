using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ChapterPortal.DAL;
using ChapterPortal.Dto;
using ChapterPortal.DAL.Models;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace ChapterPortal.Controllers
{
    [Route("[controller]")]
    [ApiController]
        
    public class HomeController : ControllerBase
    {

        private readonly IConfiguration Configuration;
        private readonly ChapterPortalDAL _chapterportalDAL;
        private readonly IWebHostEnvironment _env;
        private chapter_portalContext _context;
        private readonly IMapper _mapper;
        public HomeController(IConfiguration configuration, IWebHostEnvironment env, IMapper mapper)
        {
            Configuration = configuration;
            _chapterportalDAL = new ChapterPortalDAL();
            _env = env;
            _context = new chapter_portalContext();
            _mapper = mapper;

        }

       
        [HttpGet]
        public List<FolderModelDto> GetFolders()
        {
           var res =  _context.Folders.Include(t => t.Files).ToList();

              var data = _mapper.Map<List<FolderModelDto>>(res);
            return data;
            //return res;
        }

        [Route("GetAllFiles")]
        [HttpGet]
        public List<Files> GetAllFiles()
        {
            var res = _context.Files.ToList();
            return res;
        }

        [Route("FileUpload/{FolderId}/{IsFolderUpload}")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult FileUpload(int FolderId, bool IsFolderUpload, string path)
        {

            try {
                var files = Request.Form.Files;
                
                var folderName = Path.Combine(_env.WebRootPath, "ImageStore");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file.FileName);
                    var tempFileName = "file_" + Guid.NewGuid().ToString() + fileInfo.Extension;
                    var fullPath = Path.Combine(pathToSave, tempFileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    string FolderTitile = file.Name.Split('/')[0];
                    file.CopyTo(stream);
                    _chapterportalDAL.SaveOrUpdateFiles(FolderId, FolderTitile, file.FileName, tempFileName, fileInfo.Extension, IsFolderUpload);
                 }

                return Ok();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        

    }

}
