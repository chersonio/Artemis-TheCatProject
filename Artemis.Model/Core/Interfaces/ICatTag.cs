using Artemis.Model.Core.Entities;

namespace Artemis.Model.Core.Interfaces
{
    public interface ICatTag
    {
        int CatId { get; set; }

        CatEntity Cat { get; set; }

        int TagId { get; set; }

        TagEntity Tag { get; set; }
    }
}