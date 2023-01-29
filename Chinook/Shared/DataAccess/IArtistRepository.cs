using Chinook.Models;

namespace Chinook.Shared.DataAccess
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtist(long ArtistId);
        Task<List<Artist>> GetArtistsFiltered(string searchQuery);
    }
}
