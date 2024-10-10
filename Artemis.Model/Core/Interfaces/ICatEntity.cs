namespace Artemis.Model.Core.Entities
{
    public interface ICatEntity
    {
        /// <summary>
        /// An auto incremental unique integer that identifies a cat within the database
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Represents the id of the image returned from CaaS API
        /// </summary>
        string CatId { get; set; }

        /// <summary>
        /// Represents the width of the image returned from CaaS API
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Represents the height of the image returned from CaaS API
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Timestamp of creation of database record in UTC time
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Contains the image file in an array of bytes
        /// </summary>
        byte[] Image { get; set; }

        /// <summary>
        /// Contains the list of tags 
        /// </summary>
        ICollection<CatTag> CatTags { get; set; }

        public ICollection<TagEntity>? Tags { get; set; }
    }
}
