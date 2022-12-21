namespace DiscgogsDAL.models;

public class DiscogsSearchResponse
{
    public DiscogsSearchPagination Pagination { get; set; }
    public DiscogsSearchResult[] Results { get; set; }
}

public class DiscogsSearchPagination
{
    public uint Page { get; set; }
    public uint Pages { get; set; }
    public uint PerPage { get; set; }
    public uint Items { get; set; }
    public DiscogsSearchUrls Urls { get; set; }
}

public class DiscogsSearchUrls
{
    public string Last { get; set; }
    public string Next { get; set; }
}

public class DiscogsSearchResult
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int? MasterId { get; set; }
    public string MasterUrl { get; set; }
    public string Uri { get; set; }
    public string Title { get; set; }
    public string Thumb { get; set; }
    public string CoverImage { get; set; }
    public string ResourceUrl { get; set; }
    public string Country { get; set; }
    public int? Year { get; set; }
    public string[] Format { get; set; }
    public string[] Label { get; set; }
    public string[] Genre { get; set; }
    public string[] Style { get; set; }
    public string[] Barcode { get; set; }
    public string[] Notes { get; set; }
}