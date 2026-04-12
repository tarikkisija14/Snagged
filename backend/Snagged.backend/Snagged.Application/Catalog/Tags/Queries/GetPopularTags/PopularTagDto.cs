namespace Snagged.Application.Catalog.Tags.Queries.GetPopularTags
{
    public class PopularTagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ItemCount { get; set; }
    }
}