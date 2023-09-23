using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using DAL;


namespace LN
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<User> PutUserByIdAsync(int userId, User user);
        Task<User> DeleteUserByIdAsync(int userId);
        Task<User> LoginUser(User user);
        Task<User> GetUserByUsernameAsync(string userName);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            var newUser = await _userRepository.CreateUserAsync(user);
            return newUser;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<User> PutUserByIdAsync(int userId, User user)
        {
            return await _userRepository.PutUserByIdAsync(userId,user);
        }

        public async Task<User> DeleteUserByIdAsync(int userId)
        {
            return await _userRepository.DeleteUserByIdAsync(userId);
        }

        public async Task<User> LoginUser(User user)
        {
            return await _userRepository.LoginUser(user);
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _userRepository.GetUserByUsernameAsync(userName);
        }
    }
}