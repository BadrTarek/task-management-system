using Microsoft.Extensions.Logging;
using Domain.Interfaces.Repositories;
using Domain.Interfaces;
using AutoMapper;
using Data.Repositories.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Database.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementSystemDBContext _dbContext;
    private IDbContextTransaction _transaction;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UnitOfWork(TaskManagementSystemDBContext dbContext, IMapper mapper, ILogger<UnitOfWork> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public IUserRepository UserRepository
    {
        get
        {
            return new UserRepository(_dbContext, _mapper, _logger);
        }
    }

    public IUserTaskRepository UserTaskRepository
    {
        get
        {
            return new UserTaskRepository(_dbContext, _mapper, _logger);
        }
    }
    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
            await _transaction.DisposeAsync();

        await _dbContext.DisposeAsync();
    }
}