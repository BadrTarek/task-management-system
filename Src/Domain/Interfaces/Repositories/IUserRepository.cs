using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(User user);
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByEmailAndPassword(string email, string password);
        public Task<User?> UpdateUser(User user);
    }
}
