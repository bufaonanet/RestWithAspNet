using RestWithAspNet.Data;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithAspNet.Repository
{
    public interface IUserRepository
    {
        public User ValidateCredentials(UserDTO user);
        public User ValidateCredentials(string username);
        public bool RevokeToken(string username);
        public User RefreshUserInfo(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserDTO user)
        {
            var pass = ComputerHash(user.Password, new SHA256CryptoServiceProvider());
            return _context.Users.FirstOrDefault(p => (p.UserName == user.UserName) && (pass == p.Password));
        }

        private string ComputerHash(string password, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(p => p.Id.Equals(user.Id))) return null;

            var result = _context.Users.FirstOrDefault(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        public User ValidateCredentials(string username)
        {
            return _context.Users.FirstOrDefault(p => p.UserName == username);
        }

        public bool RevokeToken(string username)
        {
            var user = _context.Users.FirstOrDefault(p => p.UserName == username);
            if (user is null) return false;
            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }
    }
}
