namespace Chinook.Shared.DataAccess
{
    public interface IPlaylistRepository
    {
        void AddTrackToPlaylist(string userId,
                                long selectedPlaylistId,
                                string newPlaylistName,
                                long trackId);
        void DeletePlaylist(long playlistId);
        Task<List<Models.Playlist>> GetAllPlaylistForUser(string CurrentUserId);
        Task<ClientModels.Playlist> GetPlaylistById(long PlaylistId, string CurrentUserId);
        Task<Models.Playlist> GetPlaylistById(long PlaylistId);
        void RemoveTrackFromPlaylist(long trackId, long PlaylistId);
        void AddToFavorites(string userId, long trackId, FavoriteTracksConfig favTracksConfig);
        Task<List<ClientModels.PlaylistTrack>> GetTracks(long ArtistId, string CurrentUserId);
        void RenamePlaylist(long playlistId, string newPlaylistName);
    }
}