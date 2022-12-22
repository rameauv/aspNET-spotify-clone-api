using SpotifyAPI.Web;
using Microsoft.Extensions.Configuration;

namespace RealSpotifyDAL;

public class MySpotifyClient
{
    public readonly SpotifyClient SpotifyClient;

    public MySpotifyClient(IConfiguration configuration)
    {
        var spotifyConfig = configuration.GetSection("Spotify");
        var clientId = spotifyConfig.GetSection("ClientId").Value;
        var clientSecret = spotifyConfig.GetSection("ClientSecret").Value;
        if (clientId == null || clientSecret == null)
        {
            const string message = "Spotify clientId or clientSecret missing in the config";
            Console.Error.WriteLine(message);
            throw new Exception(message);
        }
        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, clientSecret));

        this.SpotifyClient = new SpotifyClient(config);
    }
}