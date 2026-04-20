using AutoMapper;
using GuildsApp.Application.DTOs.FeedDTOs;
using GuildsApp.Core.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.MappingProfiles
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<FeedPostQueryModel, FeedPostDto>();
            CreateMap<FeedGuildQueryModel, FeedGuildDto>();
            CreateMap<GuildSearchQueryModel, GuildSearchDto>();
        }
    }
}
