using Artemis.Model.Core.Entities;
using Artemis.Model.DTOs;

namespace Artemis.API.Services.Interfaces
{
    public interface ICatService
    {
        Task HandleCatsAsync();

        Task<ICatEntity> GetCatByIdAsync(int id);

        Task<IEnumerable<CatSwaggerDto>> GetCatsWithPaginationByTagNameAsync(int page, int pageSize, string tag = null);
    }
}
