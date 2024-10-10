using Artemis.Model.Core.Entities;

namespace Artemis.Data.Repositories.Interfaces
{
    public interface ICatRepository
    {
        /// <summary>
        /// Fetches cat by id as found in db
        /// </summary>
        Task<ICatEntity?> GetCatByIdAsync(int id);

        /// <summary>
        /// Fetches cats stored in db using pagination
        /// </summary>
        IEnumerable<ICatEntity> GetCatsWithPaging(int page, int pageSize, string tagName);

        /// <summary>
        /// Fetch non existing cats from a list of external cat ids
        /// </summary>
        Task<IEnumerable<string>> GetExistingCatIdsAsync(IEnumerable<string> externalCatIds);

        /// <summary>
        /// Adds new cats and saves
        /// </summary>
        Task AddCatsRangeAsync(IEnumerable<CatEntity> newCats);
    }
}