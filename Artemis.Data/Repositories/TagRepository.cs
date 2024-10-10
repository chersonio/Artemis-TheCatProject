using Artemis.Data.Repositories.Interfaces;
using Artemis.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDBContext _context;

        public TagRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddTagsRangeAsync(IEnumerable<TagEntity> newTags)
        {
            if (newTags == null || !newTags.Any())
                return;

            newTags.Select(tag => tag.Created = DateTime.Now);

            await _context.Tag.AddRangeAsync(newTags);
        }

        public async Task<IEnumerable<string>> GetExistingTagIdsFromTagNamesAsync(IEnumerable<string> fetchedTagNames)
        {
            return await _context.Tag
                .Where(tag => !fetchedTagNames.Contains(tag.Name))
                .Select(tag => tag.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<TagEntity>> GetTagsFromNamesAsync(IEnumerable<string> tagNames)
        {
            if (tagNames == null)
                throw new ArgumentNullException($"{nameof(GetTagsFromNamesAsync)} - '{nameof(tagNames)}' cannot be null.");

            return await _context.Tag
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();
        }
    }
}
