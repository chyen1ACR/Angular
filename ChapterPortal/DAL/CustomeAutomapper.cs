using AutoMapper;
using ChapterPortal.DAL.Models;
using ChapterPortal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChapterPortal.DAL
{
    public class CustomeAutomapper:Profile
    {
        public CustomeAutomapper()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Folders, FolderModelDto>();
            CreateMap<Files, FileModelDto>();
        }
    }
}
