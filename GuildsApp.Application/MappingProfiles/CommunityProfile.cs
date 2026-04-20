using AutoMapper;
using GuildsApp.Application.DTOs.CommunityDTOs;
using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.MappingProfiles
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<Community, CommunityDto>();

            CreateMap<Community, CommunityListItemDto>();

            CreateMap<CreateCommunityDto, Community>()
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Trim().ToLower()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null))
                .ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.Rules != null ? src.Rules.Trim() : null))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(_ => false));

            CreateMap<UpdateCommunityDto, Community>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null))
                .ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.Rules != null ? src.Rules.Trim() : null))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
