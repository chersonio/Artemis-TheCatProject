using Artemis.Data.Repositories.Interfaces;
using Artemis.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly ApplicationDBContext _context;

        public CatRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ICatEntity?> GetCatByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException($"{nameof(GetCatByIdAsync)} - '{nameof(id)}' must be a positive integer.");

            return await _context.Cat
                .Include(cat => cat.Tags)
                .FirstOrDefaultAsync(cat => cat.Id == id);
        }

        public IEnumerable<ICatEntity> GetCatsWithPaging(int page, int pageSize, string tagName)
        {
            if (page < 1 || pageSize < 1)
                return Enumerable.Empty<ICatEntity>();

            var cats = _context.Cat
                        .Include(ct => ct.Tags)
                        .ToHashSet();

            if (cats == null)
                return Enumerable.Empty<ICatEntity>();

            if (!string.IsNullOrEmpty(tagName))
            {
                cats = cats
                    .Where(cat => cat.CatTags
                        .Select(x => x.Tag.Name)
                        .Contains(tagName))
                    .ToHashSet();
            }

            return cats
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IEnumerable<string>> GetExistingCatIdsAsync(IEnumerable<string> externalCatIds)
        {
            return await _context.Cat
                .Where(cat => !externalCatIds.Contains(cat.CatId))
                .Select(cat => cat.CatId)
                .ToListAsync();
        }

        public async Task AddCatsRangeAsync(IEnumerable<CatEntity> newCats)
        {
            if (newCats == null || !newCats.Any())
                return;

            newCats.Select(tag => tag.Created = DateTime.Now);

            await _context.Cat.AddRangeAsync(newCats);
        }

    }
}
