using Domain.Interfaces.Repositories;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserTaskRepository UserTaskRepository { get; }
        public IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}