using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<CityDto> GetCities()
        {
            List<CityDto> citiesDto = (from city in _context.Cities
                                       select new CityDto
                                       {
                                           CityId = city.CityId,
                                           Name = city.Name,
                                           State = city.State
                                       }).ToList();

            return citiesDto;
        }

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            return new CityDto { CityId = city.CityId, Name = city.Name, State = city.State };
        }

        public CityDto UpdateCity(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();

            return new CityDto { CityId = city.CityId, Name = city.Name, State = city.State };
        }

    }
}
