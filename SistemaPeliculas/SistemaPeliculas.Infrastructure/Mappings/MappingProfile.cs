using AutoMapper;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDto>().ReverseMap();

            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.MovieId, o => o.MapFrom(s => s.MovieId ?? 0))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId ?? 0))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.FirstName + " " + s.User.LastName : null))
                .ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
        }
    }
}
