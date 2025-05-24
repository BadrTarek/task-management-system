using AutoMapper;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserTaskDBModel = Data.Database.Models.UserTask;
using UserTaskEntity = Domain.Entities.UserTask;

namespace Data.Repositories.Repositories;


public class UserTaskRepository : IUserTaskRepository
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserTaskRepository(DbContext dbContext, IMapper mapper, ILogger logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserTaskEntity?> GetUserTaskByIdAsync(int id)
    {
        _logger.LogDebug("Getting user task by ID: {Id}", id);
        var userTaskModel = await _dbContext.Set<UserTaskDBModel>().FindAsync(id);
        if (userTaskModel == null)
        {
            _logger.LogWarning("User task with ID: {Id} not found", id);
            return null;
        }
        return _mapper.Map<UserTaskEntity>(userTaskModel);
    }

    public async Task<IEnumerable<UserTaskEntity>> GetUserTasksByUserIdAsync(int userId)
    {
        _logger.LogDebug("Getting user tasks by user ID: {UserId}", userId);
        var userTaskModels = await _dbContext.Set<UserTaskDBModel>()
            .Where(ut => ut.UserId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<UserTaskEntity>>(userTaskModels);
    }

    public async Task<UserTaskEntity> CreateUserTaskAsync(UserTaskEntity userTask)
    {
        _logger.LogDebug("Creating user task for user ID: {UserId}", userTask.UserId);
        var userTaskModel = _mapper.Map<UserTaskDBModel>(userTask);
        await _dbContext.Set<UserTaskDBModel>().AddAsync(userTaskModel);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserTaskEntity>(userTaskModel);
    }

    public async Task<UserTaskEntity> UpdateUserTaskAsync(UserTaskEntity userTask)
    {
        _logger.LogDebug("Updating user task with ID: {Id}", userTask.Id);
        var userTaskModel = await _dbContext.Set<UserTaskDBModel>().FindAsync(userTask.Id);
        if (userTaskModel == null)
        {
            _logger.LogWarning("User task with ID: {Id} not found", userTask.Id);
            return null;
        }
        _mapper.Map(userTask, userTaskModel);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserTaskEntity>(userTaskModel);
    }
}
