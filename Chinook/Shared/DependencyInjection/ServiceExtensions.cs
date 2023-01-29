using Chinook.Shared.DataAccess;

namespace Chinook.Shared.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPlaylistRepository, PlaylistRepository>();
            return serviceCollection.AddScoped<IArtistRepository, ArtistRepository>();
        }
    }
}
