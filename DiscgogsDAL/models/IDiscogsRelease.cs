namespace DiscgogsDAL.models;

public interface IDiscogsRelease
{
    int Id { get; set; }
    string Status { get; set; }
    int Year { get; set; }
    string ResourceUrl { get; set; }
    string Uri { get; set; }
    IDiscogsReleaseArtist[] Artists { get; set; }
    string ArtistsSort { get; set; }
    IDiscogsLabel[] Labels { get; set; }
    IDiscogsSeries[] Series { get; set; }
    IDiscogsCompany[] Companies { get; set; }
    IDiscogsFormat[] Formats { get; set; }
    string DataQuality { get; set; }
    IDiscogsCommunity Community { get; set; }
    string[] Genres { get; set; }
    string[] Styles { get; set; }
    string Title { get; set; }
    string[] Videos { get; set; }
    IDiscogsExtraArtist[] ExtraArtists { get; set; }
    string[] Images { get; set; }
    string Country { get; set; }
    string Released { get; set; }
    string Notes { get; set; }
}

public interface IDiscogsReleaseArtist
{
    string Name { get; set; }
    string Anv { get; set; }
    string Join { get; set; }
    string Role { get; set; }
    string Tracks { get; set; }
    int Id { get; set; }
    string ResourceUrl { get; set; }
}

public interface IDiscogsLabel
{
    string Name { get; set; }
    string Catno { get; set; }
    string EntityType { get; set; }
    string EntityTypeName { get; set; }
    int Id { get; set; }
    string ResourceUrl { get; set; }
}

public interface IDiscogsSeries
{
}

public interface IDiscogsCompany
{
    string Name { get; set; }
    string Catno { get; set; }
    string EntityType { get; set; }
    string EntityTypeName { get; set; }
    int Id { get; set; }
    string ResourceUrl { get; set; }
}

public interface IDiscogsFormat
{
    string Name { get; set; }
    string Qty { get; set; }
    string[] Descriptions { get; set; }
}

public interface IDiscogsCommunity
{
    int Have { get; set; }
    int Want { get; set; }
    IDiscogsRating Rating { get; set; }
    IDiscogsSubmitter Submitter { get; set; }
    string[] Contributors { get; set; }
    string[] DataQuality { get; set; }
}

public interface IDiscogsRating
{
    int Count { get; set; }
    double Average { get; set; }
}

public interface IDiscogsSubmitter
{
    string Username { get; set; }
    string ResourceUrl { get; set; }
    int Id { get; set; }
}

public interface IDiscogsExtraArtist
{
    string Name { get; set; }
    string Anv { get; set; }
    string Role { get; set; }
    int Id { get; set; }
    string ResourceUrl { get; set; }
}
