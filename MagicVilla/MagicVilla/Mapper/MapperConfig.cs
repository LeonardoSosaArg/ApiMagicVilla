using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;

namespace MagicVilla.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Modelo fuente -> Modelo destino
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
        }
    }
}
