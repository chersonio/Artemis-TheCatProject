namespace Artemis.Model.Core.Entities
{
    public interface ITagEntity
    {
        /// <summary>
        /// An auto incremental unique integer that identifies a tag within the database
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Describes the cat’s temperament, returned from CaaS API <br />
        ///        (breeds\temperament). Note: <br />
        ///             One cat may have many tags, and many cats can share a tag <br />
        ///             Field breed/temperament contains comma-separated values. Each one of them is a tag. <br />
        ///             Search images on Cat API with breeds only.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Timestamp of creation of database record in UTC time
        /// </summary>
        public DateTime Created { get; set; }
    }
}
