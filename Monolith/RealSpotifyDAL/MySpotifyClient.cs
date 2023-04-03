using SpotifyAPI.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RealSpotifyDAL;

/// <summary>
/// MySpotifyClient is a wrapper class that creates and stores an instance of a SpotifyClient using the Spotify Web API.
/// </summary>
public class MySpotifyClient
{
    /// <summary>
    /// The Spotify client instance
    /// </summary>
    public readonly SpotifyClient SpotifyClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="MySpotifyClient"/> class.
    /// </summary>
    /// <param name="configuration">The IConfiguration object to get the Spotify clientId and clientSecret from</param>
    /// <param name="logger">The ILogger object for logging</param>
    public MySpotifyClient(IConfiguration configuration, ILogger<MySpotifyClient> logger)
    {
        var spotifyConfig = configuration.GetSection("Spotify");
        var clientId = spotifyConfig.GetSection("ClientId").Value;
        var clientSecret = spotifyConfig.GetSection("ClientSecret").Value;
        if (clientId == null || clientSecret == null)
        {
            const string message = "Spotify clientId or clientSecret missing in the config";

            logger.LogCritical(message);
            throw new Exception(message);
        }

        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, clientSecret));

        this.SpotifyClient = new SpotifyClient(config);
    }
}