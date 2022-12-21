namespace DiscgogsDAL.models;

public interface IDiscogsArtistReleases
{
    IDiscogsPagination Pagination { get; set; }
    IDiscogsArtistRelease[] Releases { get; set; }
}

public interface IDiscogsPagination
{
    int Page { get; set; }
    int Pages { get; set; }
    int PerPage { get; set; }
    int Items { get; set; }
    IDiscogsPaginationUrls Urls { get; set; }
}

public interface IDiscogsPaginationUrls
{
    string First { get; set; }
    string Prev { get; set; }
}

public interface IDiscogsArtistRelease
{
    int Id { get; set; }
    string Title { get; set; }
    string Type { get; set; }
    int MainRelease { get; set; }
    string Artist { get; set; }
    string Role { get; set; }
    string ResourceUrl { get; set; }
    int Year { get; set; }
    string Thumb { get; set; }
    IDiscogsArtistReleaseStats Stats { get; set; }
}

public interface IDiscogsArtistReleaseStats
{
    IDiscogsArtistReleaseCommunity Community { get; set; }
}

public interface IDiscogsArtistReleaseCommunity
{
    int InWantlist { get; set; }
    int InCollection { get; set; }
}
