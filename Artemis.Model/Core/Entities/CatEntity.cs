namespace Artemis.Model.Core.Entities
{
    public class CatEntity : ICatEntity
    {
        public int Id { get; set; }

        public string CatId { get; set; } = string.Empty;

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] Image { get; set; } = new byte[0];

        public DateTime Created { get; set; }

        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();

        public ICollection<TagEntity>? Tags { get; set; } = new List<TagEntity>();
    }
}
