namespace Chinook.Shared.DataAccess
{
    public interface IPlaylistRepository
    {
        void AddTrackToPlaylist(string userId,
                        long selectedPlaylistId,
                        string newPlaylistName,
                        long trackId);
        Task<ClientModels.Playlist> GetPlaylistById(long PlaylistId, string CurrentUserId);
        Task<List<ClientModels.PlaylistTrack>> GetTracks(long ArtistId, string CurrentUserId);
        Task<List<Models.Playlist>> GetAllPlaylistForUser(string CurrentUserId);
        void DeletePlaylist(long playlistId);

    }
}
