namespace Artemis.API.DTO
{
    public class CatDto
    {
        public string Id { get; set; } = string.Empty;

        public int Width { get; set; }

        public int Height { get; set; }

        public string[] Temperament { get; set; } = new string[0];

        public string Url { get; set; } = string.Empty;
    }
}
