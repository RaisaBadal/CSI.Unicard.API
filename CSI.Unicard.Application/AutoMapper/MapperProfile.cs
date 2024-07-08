using AutoMapper;
using CSI.Unicard.Application.DTOs;
using CSI.Unicard.Domain.Models;

namespace CSI.Unicard.Application.AutoMapper
{
    public class MapperProfile:Profile
    {

        public MapperProfile()
        {
            CreateMap<ProductDTO, Products>().ReverseMap();
            CreateMap<OrderDTO, Orders>().ReverseMap();
        }
    }
}
