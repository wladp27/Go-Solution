using System.Text.Json.Serialization;

namespace GoWeb.Shared.Features.Map.Components.Shared
{
 


   
    public record MapFeature
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = "Feature";

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("geometry")]
        public MapGeometry Geometry { get; init; }

        [JsonPropertyName("properties")]
        public MapProperties Properties { get; init; }
    }

   
    public record MapGeometry
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = "Point";

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; init; }
    }

   
    public record MapProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }
    }

    public record SearchResult(double Longitude, double Latitude, string Name, string Address);
}
