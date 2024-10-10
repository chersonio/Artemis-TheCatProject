using Artemis.API.DTO;
using Artemis.API.Helpers;
using Artemis.API.Services.Interfaces;
using Artemis.Data.UnitOfWork;
using Artemis.Model.Core.Entities;
using Artemis.Model.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Artemis.API.Services
{
    public class CatService : ICatService
    {
        const int BatchSize = 50;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly CatJsonConverter _converter;
        private readonly ApiSettings _apiSettings;

        public CatService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiSettings.Token);
            _converter = new CatJsonConverter();
        }

        public async Task HandleCatsAsync()
        {
            // Fetch cats
            List<CatDto> fetchedCats = await RetrieveCatsFromCatApiAsync();

            var test = _unitOfWork.Cats;

            if (fetchedCats == null || fetchedCats.Count() < 1)
                return;

            await AddFetchedItemsAsync(fetchedCats);
        }

        /// <summary>
        /// Uses cat api to get random cats. It returns them in CatDto 
        /// </summary>
        private async Task<List<CatDto>> RetrieveCatsFromCatApiAsync()
        {
            var fetchedCats = new List<CatDto>();
            var response = await _httpClient.GetAsync(_apiSettings.ApiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                fetchedCats = _converter.ExtractCatInfo(jsonString);
            }

            return fetchedCats;
        }

        private async Task AddFetchedItemsAsync(IEnumerable<CatDto> fetchedCats)
        {
            if (fetchedCats == null || !fetchedCats.Any())
                throw new ArgumentException("No cats were fetched from the API.");

            IEnumerable<CatDto> newCats = await GetNonExistingCatIdsAsync(fetchedCats);

            if (!newCats.Any())
                return;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var newCatEntities = new List<CatEntity>();
                var newTagsToExclude = new List<string>();

                foreach (var catDto in newCats)
                {
                    List<TagEntity> newTagEntities = (await PrepareTagEntitiesAsync(catDto, newTagsToExclude)).ToList();
                    await _unitOfWork.Tags.AddTagsRangeAsync(newTagEntities);

                    var newCat = await PrepareCatEntitiesAsync(catDto);
                    newCatEntities.Add(newCat);

                    foreach (var newTagEntity in newTagEntities)
                    {
                        newCat.Tags.Add(newTagEntity);
                    }

                    newTagsToExclude.AddRange(newTagEntities.Select(newTag => newTag.Name));
                }

                await _unitOfWork.Cats.AddCatsRangeAsync(newCatEntities);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private async Task<IEnumerable<CatDto>> GetNonExistingCatIdsAsync(IEnumerable<CatDto> fetchedCats)
        {
            var fetchedExternalCatIds = fetchedCats.Select(cat => cat.Id);

            var existingCatIds = await _unitOfWork.Cats.GetExistingCatIdsAsync(fetchedExternalCatIds);

            var newCats = fetchedCats.Where(cat => !existingCatIds.Contains(cat.Id));
            return newCats;
        }

        private async Task<IEnumerable<string>> GetNonExistingTagIdsAsync(IEnumerable<string> fetchedTagNames)
        {
            var existingTagNames = await _unitOfWork.Tags.GetExistingTagIdsFromTagNamesAsync(fetchedTagNames);

            var newTags = fetchedTagNames.Where(tagName => !existingTagNames.Contains(tagName));
            return newTags;
        }

        private async Task<IEnumerable<TagEntity>> PrepareTagEntitiesAsync(CatDto catDto, IEnumerable<string> excludeExistingNamesInMemory)
        {
            // Check existing in DB
            var newTagNames = await GetNonExistingTagIdsAsync(catDto.Temperament);

            // Check existing in memory
            newTagNames = newTagNames.Where(nt => !excludeExistingNamesInMemory.Contains(nt));

            var tagEntities = newTagNames.Select(tagName => new TagEntity { Name = tagName });

            return tagEntities;
        }

        private async Task<CatEntity> PrepareCatEntitiesAsync(CatDto catDto)
        {
            var catEntity = _mapper.Map<CatEntity>(catDto);

            // Download image to byte[]
            var imageBytes = await _httpClient.GetByteArrayAsync(catDto.Url);
            catEntity.Image = imageBytes;

            return catEntity;
        }

        public async Task<ICatEntity> GetCatByIdAsync(int id)
        {
            return await _unitOfWork.Cats.GetCatByIdAsync(id);
        }

        public async Task<IEnumerable<CatSwaggerDto>> GetCatsWithPaginationByTagNameAsync(int page, int pageSize, string tagName = null)
        {
            if (page < 1 || pageSize < 1)
                return Enumerable.Empty<CatSwaggerDto>();

            var cats = _unitOfWork.Cats.GetCatsWithPaging(page, pageSize, tagName);

            if (cats is null || !cats.Any())
                return Enumerable.Empty<CatSwaggerDto>();

            var viewCats = new List<CatSwaggerDto>();

            for (int i = 0; i < pageSize; i += BatchSize)
            {
                var catsBatch = cats
                    .Take(BatchSize)
                    .Select(cat => new CatSwaggerDto
                    {
                        Id = cat.Id,
                        Tags = cat.CatTags.Select(ct => new TagSwaggerDto
                        {
                            Id = ct.Tag.Id,
                            Name = ct.Tag.Name,
                            Created = ct.Tag.Created,
                        }).ToList()
                    });

                viewCats.AddRange(catsBatch);
            }

            return viewCats;
        }
    }
}
