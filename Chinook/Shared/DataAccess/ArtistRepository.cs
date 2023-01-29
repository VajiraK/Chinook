using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Shared.DataAccess
{
    public class ArtistRepository : IArtistRepository
    {
        readonly IDbContextFactory<ChinookContext> _DbFactory;

        public ArtistRepository(IDbContextFactory<ChinookContext> dbFactory)
        {
            _DbFactory = dbFactory;
        }

        public async Task<Artist> GetArtist(long ArtistId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();
            var Artist = DbContext.Artists.SingleOrDefault(a => a.ArtistId == ArtistId);
            return Artist;
        }
    }
}
