using Application.Services.Interfaces;
using Application.UserTaskDtos.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using AutoMapper;

namespace Application.Services;

public class UserTaskService : IUserTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserTaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserTaskDto>> GetUserTasksAsync(int userId)
    {
        var tasks = await _unitOfWork.UserTaskRepository.GetUserTasksByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<UserTaskDto>>(tasks);
    }

    public async Task<UserTaskDto> CreateTaskAsync(CreateUserTaskDto createTaskDto)
    {
        var task = _mapper.Map<UserTask>(createTaskDto);
        task.Status = UserTaskStatuses.ToDo;
        var createdTask = await _unitOfWork.UserTaskRepository.CreateUserTaskAsync(task);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserTaskDto>(createdTask);
    }

    public async Task<UserTaskDto> UpdateTaskAsync(UpdateUserTaskDto updateTaskDto)
    {
        var existingTask = await _unitOfWork.UserTaskRepository.GetUserTaskByIdAsync(updateTaskDto.Id);
        if (existingTask == null)
        {
            throw new InvalidOperationException("Task not found");
        }
        // Updatable properties
        existingTask.Title = updateTaskDto.Title;
        existingTask.Description = updateTaskDto.Description;
        existingTask.DueDate = updateTaskDto.DueDate;
        existingTask.Status = updateTaskDto.Status;
        var updatedTask = await _unitOfWork.UserTaskRepository.UpdateUserTaskAsync(existingTask);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserTaskDto>(updatedTask);
    }
}