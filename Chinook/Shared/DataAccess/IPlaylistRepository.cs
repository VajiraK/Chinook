namespace Chinook.Shared.DataAccess
{
    public interface IPlaylistRepository
    {
        Task<ClientModels.Playlist> GetPlaylistById(long PlaylistId, string CurrentUserId);
        Task<List<ClientModels.PlaylistTrack>> GetTracks(long ArtistId, string CurrentUserId);

    }
}
