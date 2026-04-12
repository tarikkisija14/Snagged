namespace Snagged.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();
    }
}