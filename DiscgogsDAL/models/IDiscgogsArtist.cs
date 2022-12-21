namespace DiscgogsDAL.models;

public interface IDiscogsArtist
{
    string Name { get; set; }
    int Id { get; set; }
    string ResourceUrl { get; set; }
    string Uri { get; set; }
    string ReleasesUrl { get; set; }
    DiscogsImage[] Images { get; set; }
    string Realname { get; set; }
    string Profile { get; set; }
    string[] Urls { get; set; }
    string[] Namevariations { get; set; }
    DiscogsArtistAlias[] Aliases { get; set; }
    string DataQuality { get; set; }
}

public interface DiscogsImage
{
    string Type { get; set; }
    string Uri { get; set; }
    string ResourceUrl { get; set; }
    string Uri150 { get; set; }
    int Width { get; set; }
    int Height { get; set; }
}

public interface DiscogsArtistAlias
{
    int Id { get; set; }
    string Name { get; set; }
    string ResourceUrl { get; set; }
}
