using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;
using Domain.Interfaces;
using AutoMapper;
using Data.Repositories.Repositories;

namespace Data.Database.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementSystemDBContext _dbContext;
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

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}