using Artemis.Model.Core.Interfaces;

namespace Artemis.Model.Core.Entities
{
    public class CatTag : ICatTag
    {
        public int Id { get; set; }

        public int CatId { get; set; }

        public int TagId { get; set; }

        public CatEntity Cat { get; set; }

        public TagEntity Tag { get; set; }
    }
}
