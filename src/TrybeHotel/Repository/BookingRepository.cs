using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Mvc;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var room = GetRoomById(booking.RoomId);
            if (room.Capacity < booking.GuestQuant)
                throw new InvalidOperationException("Guest quantity over room capacity");

            var user = (from u in _context.Users
                        where u.Email == email
                        select u).FirstOrDefault();

            Booking newBooking = new()
            {
                CheckIn = booking.Checkin,
                CheckOut = booking.Checkout,
                GuestQuant = booking.GuestQuant,
                UserId = user!.UserId,
                RoomId = booking.RoomId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return new BookingResponse
            {
                BookingId = newBooking.BookingId,
                Checkin = newBooking.CheckIn,
                Checkout = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = (from h in _context.Hotels
                             where h.HotelId == room.HotelId
                             join c in _context.Cities on h.CityId equals c.CityId
                             select new HotelDto
                             {
                                 HotelId = h.HotelId,
                                 Name = h.Name,
                                 Address = h.Address,
                                 CityId = h.CityId,
                                 CityName = c.Name,
                                 State = c.State
                             }).FirstOrDefault()
                }
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = (from b in _context.Bookings
                           where b.BookingId == bookingId
                           select b).FirstOrDefault();

            var user = (from u in _context.Users
                        where u.Email == email
                        select u).FirstOrDefault();

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.UserId != user!.UserId)
                throw new UnauthorizedAccessException();

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                Checkin = booking.CheckIn,
                Checkout = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = (from r in _context.Rooms
                        where r.RoomId == booking.RoomId
                        join h in _context.Hotels on r.HotelId equals h.HotelId
                        join c in _context.Cities on h.CityId equals c.CityId
                        select new RoomDto
                        {
                            RoomId = r.RoomId,
                            Name = r.Name,
                            Capacity = r.Capacity,
                            Image = r.Image,
                            Hotel = new HotelDto
                            {
                                HotelId = h.HotelId,
                                Name = h.Name,
                                Address = h.Address,
                                CityId = h.CityId,
                                CityName = c.Name,
                                State = c.State
                            }
                        }).FirstOrDefault()!
            };
        }

        public Room GetRoomById(int RoomId)
        {
            var room = (from r in _context.Rooms
                        where r.RoomId == RoomId
                        select r).FirstOrDefault();

            if (room != null)
                return room;

            throw new KeyNotFoundException("Room not found");
        }
    }
}