using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Api.Services
{
    public class UserService : IUserService
    {
        private YASDMApiDbContext _db;

        public UserService(YASDMApiDbContext db)
        {
            _db = db;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = await _db.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user is null)
            {
                return null;
            }

            if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return user;
            }

            return null;

        }

        public async Task<User> Create(AuthRegisterDTO registerDTO)
        {
            if (string.IsNullOrWhiteSpace(registerDTO.Password))
                throw new ApiException("Password is required");
            
            if(string.IsNullOrWhiteSpace(registerDTO.Username))
            {
                throw new ApiException("Username can't be empty");
            }

            if(!Utils.IsValidEmail(registerDTO.Email))
            {
                throw new ApiException("Invalid email");
            }

            if (await _db.Users.AnyAsync(x => x.UserName == registerDTO.Username))
                throw new ApiException("Username \"" + registerDTO.Username + "\" is already taken");


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(registerDTO.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _db.Users.AddAsync(user);

            await _db.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null)
            {
                throw new ApiNotFoundException();
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User> GetEagerById(int id)
        {
            return await _db.Users.Include(u => u.UserRooms).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateCredentials(int id, AuthRegisterDTO registerDTO)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                throw new ApiNotFoundException();

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(registerDTO.Username) && registerDTO.Username != user.UserName)
            {
                // throw error if the new username is already taken
                if (await _db.Users.AnyAsync(x => x.UserName == registerDTO.Username))
                    throw new ApiException($"Username {registerDTO.Username} is already taken");

                user.UserName = registerDTO.Username;
            }


            // update password if provided
            if (!string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(registerDTO.Password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _db.SaveChanges();
        }

        public async Task UpdateUser(int id, UserDTO updateDTO)
        {

            var user = await _db.Users.FindAsync(id);

            if (user is null)
            {
                throw new ApiNotFoundException();
            }

            if(!Utils.IsValidEmail(updateDTO.Email)){
                throw new ApiException("Invalid email!");
            }

            user.FirstName = updateDTO.FirstName;
            user.LastName = updateDTO.LastName;
            user.UserName = updateDTO.Username;
            user.Email = updateDTO.Email;

            await _db.SaveChangesAsync();

        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ApiException("Password can't be empty!");
            if (string.IsNullOrWhiteSpace(password)) throw new ApiException("Password cannot be empty or whitespace only string.");
            if (storedHash.Length != 64) throw new ApiException("Invalid length of password hash (64 bytes expected).");
            if (storedSalt.Length != 128) throw new ApiException("Invalid length of password salt (128 bytes expected).");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("Password can't be empty!");
            if (string.IsNullOrWhiteSpace(password)) throw new ApiException("Password cannot be empty or whitespace only string.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}