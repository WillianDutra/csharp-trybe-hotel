using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            List<HotelDto> hotelsDto = (from hotel in _context.Hotels
                                        join city in _context.Cities on hotel.CityId equals city.CityId
                                        select new HotelDto
                                        {
                                            HotelId = hotel.HotelId,
                                            Name = hotel.Name,
                                            Address = hotel.Address,
                                            CityId = hotel.CityId,
                                            CityName = city.Name,
                                            State = city.State
                                        }).ToList();

            return hotelsDto;
        }

        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var city = (from c in _context.Cities
                        where c.CityId == hotel.CityId
                        select c).FirstOrDefault();

            return new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = city.Name,
                State = city.State
            };
        }
    }
}