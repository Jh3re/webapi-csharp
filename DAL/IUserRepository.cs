using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Tools;
using Microsoft.EntityFrameworkCore;



namespace DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<User> PutUserByIdAsync(int userId, User user);
        Task<User> DeleteUserByIdAsync(int userId);
        Task<User> LoginUser(User user);
        Task<User> GetUserByUsernameAsync(string userName);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            string encryptedPassword = PasswordEncryptor.EncryptPassword(user.Contraseña);

            user.Contraseña = encryptedPassword;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user;
        }

        public async Task<User> PutUserByIdAsync(int userId, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginUser(User user)
        {
            var userLogin = await _context.Users.FirstOrDefaultAsync(e => e.Correo == user.Correo && e.Contraseña == user.Contraseña);

            return userLogin;
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            var username = await _context.Users.FirstOrDefaultAsync(u => u.Correo == userName);
            return username;
        }

    }
}