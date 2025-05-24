using Application.UserTaskDtos.Dtos;

namespace Application.Services.Interfaces;

public interface IUserTaskService
{
    Task<IEnumerable<UserTaskDto>> GetUserTasksAsync(int userId);
    Task<UserTaskDto> CreateTaskAsync(CreateUserTaskDto createTaskDto);
    Task<UserTaskDto> UpdateTaskAsync(UpdateUserTaskDto updateTaskDto);
}