namespace DiscgogsDAL.Repositories;

public class BaseRepository
{
    protected readonly HttpClient _client;
    protected readonly string _basePath;
    protected readonly string _key;
    protected readonly string _secret;

    protected BaseRepository()
    {
        this._client = new HttpClient();
        this._basePath = "https://api.discogs.com";
        this._key = "yLYoVwMnRzVcEMWEWRhB";
        this._secret = "HRDFgVONHJpPXjXKpAiHdtAcaePOesFj";
    }
}