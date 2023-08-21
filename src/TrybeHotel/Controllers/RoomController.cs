using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId)
        {
            var rooms = _repository.GetRooms(HotelId);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult PostRoom([FromBody] Room room)
        {
            var createdRoom = _repository.AddRoom(room);
            return Created("", createdRoom);
        }

        [HttpDelete("{RoomId}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int RoomId)
        {
            _repository.DeleteRoom(RoomId);
            return NoContent();
        }
    }
}