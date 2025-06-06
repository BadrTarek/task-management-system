using AutoMapper;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserDBModel = Data.Database.Models.User;
using UserEntity = Domain.Entities.User;

namespace Data.Repositories.Repositories;


public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserRepository(DbContext dbContext, IMapper mapper, ILogger logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserEntity> CreateUser(UserEntity user)
    {
        _logger.LogDebug("Creating user with email: {Email}", user.Email);
        var userModel = _mapper.Map<UserDBModel>(user);
        await _dbContext.Set<UserDBModel>().AddAsync(userModel);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserEntity>(userModel);
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        _logger.LogDebug("Getting user by email: {Email}", email);
        var userModel = await _dbContext.Set<UserDBModel>()
            .FirstOrDefaultAsync(u => u.Email == email);
        if (userModel == null)
        {
            _logger.LogWarning("User with email: {Email} not found", email);
            return null;
        }
        return _mapper.Map<UserEntity>(userModel);
    }
}
