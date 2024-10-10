namespace Artemis.Model.Core.Entities
{
    public class TagEntity : ITagEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public ICollection<CatEntity> Cats { get; set; } = new List<CatEntity>();

        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
