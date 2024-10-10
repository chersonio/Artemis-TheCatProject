using Artemis.API.DTO;
using Artemis.Model.Core.Entities;
using AutoMapper;

namespace Artemis.API.AutoMapper.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatDto, CatEntity>()
                .ForMember(dest => dest.CatId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CatTags, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore());
        }
    }
}
