using AutoMapper;
using EnigmaVault.Secret.Domain.Models;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Mappers.Profiles
{
    internal sealed class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
        }
    }
}