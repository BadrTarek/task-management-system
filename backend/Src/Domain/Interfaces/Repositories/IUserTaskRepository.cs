using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserTaskRepository
    {
        public Task<UserTask?> GetUserTaskByIdAsync(int id);
        public Task<IEnumerable<UserTask>> GetUserTasksByUserIdAsync(int userId);
        public Task<UserTask> CreateUserTaskAsync(UserTask userTask);
        public Task<UserTask> UpdateUserTaskAsync(UserTask userTask);
    }
}