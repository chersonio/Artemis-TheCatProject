using Artemis.Model.Core.Entities;

namespace Artemis.Data.Repositories.Interfaces
{
    public interface ITagRepository
    {
        /// <summary>
        /// Adds new tags and saves
        /// </summary>
        Task AddTagsRangeAsync(IEnumerable<TagEntity> newTags);

        Task<IEnumerable<string>> GetExistingTagIdsFromTagNamesAsync(IEnumerable<string> fetchedTagNames);

        Task<IEnumerable<TagEntity>> GetTagsFromNamesAsync(IEnumerable<string> enumerable);
    }
}