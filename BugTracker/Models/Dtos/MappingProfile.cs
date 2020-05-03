using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bug, BugDto>();
            CreateMap<BugDto, Bug>();
        }
    }
}
