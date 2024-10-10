using Artemis.API.DTO;
using System.Text.Json;

namespace Artemis.API.Helpers
{
    /// <summary>
    /// Converts Json from http requests to CatDto
    /// </summary>
    public class CatJsonConverter
    {
        /// <summary>
        /// Matches properties needed for CatDto
        /// </summary>
        public List<CatDto> ExtractCatInfo(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                var root = document.RootElement.EnumerateArray();

                var catDtos = new List<CatDto>();

                foreach (var item in root)
                {
                    var id = item.GetProperty("id").GetString();
                    var imageUrl = item.GetProperty("url").GetString();
                    var height = item.GetProperty("height").GetInt32();
                    var width = item.GetProperty("width").GetInt32();
                    var breeds = item.GetProperty("breeds").EnumerateArray();

                    string[] temperament = breeds.FirstOrDefault()
                        .GetProperty("temperament")
                        .GetString()
                        .Split(", ");

                    catDtos.Add(new CatDto
                    {
                        Id = id,
                        Temperament = temperament,
                        Url = imageUrl,
                        Height = height,
                        Width = width,
                    });
                }

                return catDtos;
            }
        }
    }
}
