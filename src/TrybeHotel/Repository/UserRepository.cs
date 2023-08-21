using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new System.NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var validUser = (from user in _context.Users
                             where user.Email == login.Email &&
                             user.Password == login.Password
                             select new UserDto
                             {
                                 UserId = user.UserId,
                                 Name = user.Name!,
                                 Email = user.Email!,
                                 UserType = user.UserType!
                             }).FirstOrDefault();

            if (validUser == null)
                throw new InvalidOperationException("Incorrect e-mail or password");

            return validUser;
        }
        public UserDto Add(UserDtoInsert user)
        {
            if (GetUserByEmail(user.Email) != null)
                throw new InvalidOperationException("User email already exists");

            User newUser = new()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = (from u in _context.Users
                        where u.Email == userEmail
                        select new UserDto
                        {
                            UserId = u.UserId,
                            Name = u.Name!,
                            Email = u.Email!,
                            UserType = u.UserType!
                        }).FirstOrDefault();

            return user!;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = (from u in _context.Users
                         select new UserDto
                         {
                             UserId = u.UserId,
                             Name = u.Name,
                             Email = u.Email,
                             UserType = u.UserType
                         }).ToList();

            return users;
        }
    }
}