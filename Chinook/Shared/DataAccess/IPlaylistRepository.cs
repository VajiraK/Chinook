namespace Chinook.Shared.DataAccess
{
    public interface IPlaylistRepository
    {
        Task<ClientModels.Playlist> GetPlaylistById(long PlaylistId, string CurrentUserId);

    }
}
