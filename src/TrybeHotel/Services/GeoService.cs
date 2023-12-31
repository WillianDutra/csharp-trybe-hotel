using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrybeHotel.Dto;
using TrybeHotel.Repository;

namespace TrybeHotel.Services
{
    public class GeoService : IGeoService
    {
        private readonly HttpClient _client;
        public GeoService(HttpClient client)
        {
            _client = client;
            client.DefaultRequestHeaders.Add("User-Agent", "TrybeHotel/v1.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<object> GetGeoStatus()
        {
            var response = await _client.GetAsync("https://nominatim.openstreetmap.org/status.php?format=json");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto)
        {
            string URL = $"https://nominatim.openstreetmap.org/search?street={geoDto.Address}&city={geoDto.City}&country=Brazil&state={geoDto.State}&format=json&limit=1";
            var response = await _client.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<GeoDtoResponse[]>();
                return content.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository)
        {
            var userDistance = await GetGeoLocation(geoDto);

            var hotels = repository.GetHotels();
            var hotelsByDistance = new List<GeoDtoHotelResponse>();
            foreach (var hotel in hotels)
            {
                var hotelDistance = await GetGeoLocation(new GeoDto
                {
                    Address = hotel.Address,
                    City = hotel.CityName,
                    State = hotel.State
                });

                hotelsByDistance.Add(new GeoDtoHotelResponse
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityName = hotel.CityName,
                    State = hotel.State,
                    Distance = CalculateDistance(userDistance.lat, userDistance.lon, hotelDistance.lat, hotelDistance.lon)
                });
            }

            return hotelsByDistance.OrderBy(h => h.Distance).ToList();
        }

        public int CalculateDistance(string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny)
        {
            double latOrigin = double.Parse(latitudeOrigin.Replace('.', ','));
            double lonOrigin = double.Parse(longitudeOrigin.Replace('.', ','));
            double latDestiny = double.Parse(latitudeDestiny.Replace('.', ','));
            double lonDestiny = double.Parse(longitudeDestiny.Replace('.', ','));
            double R = 6371;
            double dLat = radiano(latDestiny - latOrigin);
            double dLon = radiano(lonDestiny - lonOrigin);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return int.Parse(Math.Round(distance, 0).ToString());
        }

        public double radiano(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}