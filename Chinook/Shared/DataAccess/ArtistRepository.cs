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

        /// <summary>
        /// If searchQuery is blank returns all artists
        /// </summary>
        /// <returns>Artist List</returns>
        public async Task<List<Artist>> GetArtistsFiltered(string searchQuery)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();

            if (searchQuery == string.Empty)
            {
                return DbContext.Artists.ToList();
            }
            else
            {
                return DbContext.Artists.Where(a => a.Name.Contains(searchQuery)).ToList();
            }

        }
    }
}
