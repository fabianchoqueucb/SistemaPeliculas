using AutoMapper;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieDto, Movie>();

            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.MovieId, o => o.MapFrom(s => s.MovieId ?? 0))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId ?? 0));
            CreateMap<ReviewDto, Review>();
        }
    }
}
