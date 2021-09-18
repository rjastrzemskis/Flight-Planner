using System.Text.Json.Serialization;

namespace FlightPlanner.Web.Models
{
    public class Airport
    {
        public string Country { get; set; }
        public string City { get; set; }
        [JsonPropertyName("Airport")] public string AirportCode { get; set; }
    }
}