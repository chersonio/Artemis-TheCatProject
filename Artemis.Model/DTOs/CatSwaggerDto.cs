namespace Artemis.Model.DTOs
{
    public class CatSwaggerDto
    {
        public int Id { get; set; }
        public IEnumerable<TagSwaggerDto> Tags { get; set; } = new List<TagSwaggerDto>();
    }
}
