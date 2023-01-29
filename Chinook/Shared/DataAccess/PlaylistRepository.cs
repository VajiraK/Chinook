using Chinook.Models;
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

        public async Task<Models.Playlist> GetPlaylistById(long PlaylistId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();
            return DbContext.Playlists.SingleOrDefault(pl => pl.PlaylistId == PlaylistId);
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

        public async Task<List<ClientModels.PlaylistTrack>> GetTracks(long ArtistId, string CurrentUserId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();

            List<ClientModels.PlaylistTrack> Tracks = DbContext.Tracks.Where(a => a.Album.ArtistId == ArtistId)
                                        .Include(a => a.Album)
                                        .Select(t => new ClientModels.PlaylistTrack()
                                        {
                                            AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                                            TrackId = t.TrackId,
                                            TrackName = t.Name,
                                            IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == "Favorites")).Any()
                                        })
                                        .ToList();

            return Tracks;
        }

        public async Task<List<Models.Playlist>> GetAllPlaylistForUser(string CurrentUserId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();
            var playlists = DbContext.UserPlaylists
                .Where(up => up.UserId == CurrentUserId).Select(up => up.Playlist).ToList();
            return playlists;
        }

        public async void DeletePlaylist(long playlistId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();

            //Remove playlist reference for userPlaylist
            var userPlaylist = DbContext.UserPlaylists.SingleOrDefault(up => up.PlaylistId == playlistId);
            DbContext.UserPlaylists.Remove(userPlaylist);

            //Get tracks populated playlist
            var playlist = DbContext.Playlists.Include(t => t.Tracks)
                                                            .SingleOrDefault(p => p.PlaylistId == playlistId);

            //Clear tracks for the playlist from PlalylistTracks
            playlist.Tracks.Clear();

            //Delete the playlist
            DbContext.Playlists.Remove(playlist);

            await DbContext.SaveChangesAsync();
        }

        public async void AddTrackToPlaylist(string userId,
                                        long selectedPlaylistId,
                                        string newPlaylistName,
                                        long trackId)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();
            //Track will be added to this playlist
            long currentPlaylistId = selectedPlaylistId;

            //Check if user needs to create a new playlist
            if (newPlaylistName != null)
            {
                //Get next playlist id
                long nextPlaylistId = DbContext.Playlists.Count() + 1;
                currentPlaylistId = nextPlaylistId;

                Models.Playlist playlist = new()
                {
                    PlaylistId = nextPlaylistId,
                    Name = newPlaylistName
                };

                //Add new playlist
                DbContext.Playlists.Add(playlist);

                //Add mapping to UserPlaylist
                UserPlaylist up = new()
                {
                    UserId = userId,
                    PlaylistId = nextPlaylistId
                };

                //Add new playlist to the DB
                DbContext.UserPlaylists.Add(up);
                await DbContext.SaveChangesAsync();
            }

            //Get tracks populated playlist
            var newPlaylist = DbContext.Playlists.Include(t => t.Tracks)
                                                            .SingleOrDefault(p => p.PlaylistId == currentPlaylistId);

            //Add track to the playlist
            var track = DbContext.Tracks.SingleOrDefault(t => t.TrackId == trackId);
            newPlaylist.Tracks.Add(track);

            DbContext.SaveChangesAsync();
        }

        public async void AddToFavorites(string userId, long trackId, FavoriteTracksConfig favTracksConfig)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();

            //Try to get favorit playlist item
            var favoritePlaylist = DbContext.Playlists.SingleOrDefault(pl => pl.PlaylistId == favTracksConfig.FavoriteTracksId);

            if (favoritePlaylist == null)
            {
                //Need to create a favorit playlist
                favoritePlaylist = new()
                {
                    PlaylistId = favTracksConfig.FavoriteTracksId,
                    Name = favTracksConfig.FavoriteTracksName
                };

                DbContext.Playlists.Add(favoritePlaylist);
                await DbContext.SaveChangesAsync();
            }

            //Get tracks populated playlist
            favoritePlaylist = DbContext.Playlists.Include(t => t.Tracks)
                                                            .SingleOrDefault(p => p.PlaylistId == favTracksConfig.FavoriteTracksId);


            //Add track to the favorites playlist
            var track = DbContext.Tracks.SingleOrDefault(t => t.TrackId == trackId);
            favoritePlaylist.Tracks.Add(track);

            //Add track to favorites
            Models.PlaylistTrack playlistTrack = new()
            {
                PlaylistId = favTracksConfig.FavoriteTracksId,
                TrackId = trackId
            };

            DbContext.SaveChangesAsync();
        }

        public async void RenamePlaylist(long playlistId, string newPlaylistName)
        {
            var DbContext = await _DbFactory.CreateDbContextAsync();
            var currentPlaylit = await this.GetPlaylistById(playlistId);
            currentPlaylit.Name = newPlaylistName;
            DbContext.Playlists.Update(currentPlaylit);
            DbContext.SaveChanges();
        }
    }
}
