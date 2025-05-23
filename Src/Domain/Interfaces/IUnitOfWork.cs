
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserTaskRepository UserTaskRepository { get; }
        public IUserRepository UserRepository { get; }
        public Task CommitAsync();
        public Task RollbackAsync();
    }
}