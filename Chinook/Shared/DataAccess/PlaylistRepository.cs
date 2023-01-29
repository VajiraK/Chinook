using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Chinook.Shared.DataAccess
{
    public class PlaylistRepository : IPlaylistRepository
    {
        readonly IDbContextFactory<ChinookContext> _DbFactory;

        public PlaylistRepository(IDbContextFactory<ChinookContext> dbFactory)
        {
            _DbFactory = dbFactory;
        }

        public async Task<ClientModels.Playlist> GetPlaylistById(long PlaylistId, string CurrentUserId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();

            ClientModels.Playlist playlist = DbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == PlaylistId)
                .Select(p => new ClientModels.Playlist()
                {
                    Name = p.Name,
                    Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrack()
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == "Favorites")).Any()
                    }).ToList()
                })
                .FirstOrDefault();

            return playlist;
        }
    }
}
