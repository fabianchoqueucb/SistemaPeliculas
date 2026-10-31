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
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.FirstName + " " + s.User.LastName : null));

            CreateMap<ReviewDto, Review>()
                .ForMember(d => d.Movie, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore());

            CreateMap<Comment, CommentDto>().ReverseMap();

            CreateMap<Actor, ActorDto>()
                .ForMember(d => d.MovieTitle, o => o.MapFrom(s => s.Movie != null ? s.Movie.Title : null));

            CreateMap<ActorDto, Actor>()
                .ForMember(d => d.Movie, o => o.Ignore());

            CreateMap<Watchlist, WatchlistDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.FirstName + " " + s.User.LastName : null))
                .ForMember(d => d.MovieTitle, o => o.MapFrom(s => s.Movie != null ? s.Movie.Title : null))
                .ForMember(d => d.MovieGenre, o => o.MapFrom(s => s.Movie != null ? s.Movie.Genre : null))
                .ForMember(d => d.MovieReleaseDate, o => o.MapFrom(s => s.Movie != null ? (DateTime?)s.Movie.ReleaseDate : null));

            CreateMap<WatchlistDto, Watchlist>()
                .ForMember(d => d.User, o => o.Ignore())
                .ForMember(d => d.Movie, o => o.Ignore());

            CreateMap<CrearWatchlistDto, Watchlist>()
                .ForMember(d => d.User, o => o.Ignore())
                .ForMember(d => d.Movie, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.DateAdded, o => o.Ignore());
        }
    }
}
