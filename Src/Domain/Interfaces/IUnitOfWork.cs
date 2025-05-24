using Domain.Interfaces.Repositories;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserTaskRepository UserTaskRepository { get; }
        public IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}