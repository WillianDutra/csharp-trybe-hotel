using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            List<RoomDto> rooms = (from hotel in _context.Hotels
                                   where hotel.HotelId == HotelId
                                   join room in _context.Rooms on hotel.HotelId equals room.HotelId
                                   join city in _context.Cities on hotel.CityId equals city.CityId
                                   select new RoomDto
                                   {
                                       RoomId = room.RoomId,
                                       Name = room.Name,
                                       Capacity = room.Capacity,
                                       Image = room.Image,
                                       Hotel = new HotelDto
                                       {
                                           HotelId = hotel.HotelId,
                                           Name = hotel.Name,
                                           Address = hotel.Address,
                                           CityId = hotel.CityId,
                                           CityName = city.Name,
                                       }
                                   }).ToList();

            return rooms;
        }

        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return (from hotel in _context.Hotels
                    where hotel.HotelId == room.HotelId
                    join city in _context.Cities on hotel.CityId equals city.CityId
                    select new RoomDto
                    {
                        RoomId = room.RoomId,
                        Name = room.Name,
                        Capacity = room.Capacity,
                        Image = room.Image,
                        Hotel = new HotelDto
                        {
                            HotelId = hotel.HotelId,
                            Name = hotel.Name,
                            Address = hotel.Address,
                            CityId = hotel.CityId,
                            CityName = city.Name,
                        }
                    }).FirstOrDefault() ?? null!;
        }

        public void DeleteRoom(int RoomId)
        {
            var room = _context.Rooms.Find(RoomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
        }
    }
}