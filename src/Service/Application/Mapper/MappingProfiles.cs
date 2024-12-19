using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
