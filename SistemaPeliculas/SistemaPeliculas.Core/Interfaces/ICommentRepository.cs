using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task InsertComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Comment comment);
    }
}

