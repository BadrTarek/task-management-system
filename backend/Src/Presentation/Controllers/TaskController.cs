using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.UserTaskDtos.Dtos;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IUserTaskService _taskService;

    public TaskController(IUserTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetUserTasks()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in token"));
        var tasks = await _taskService.GetUserTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<UserTaskDto>> CreateTask([FromBody] CreateUserTaskDto createTaskDto)
    {
        // Fill the UserId from the token
        createTaskDto.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in token"));

        // Validate the DTO        
        var context = new ValidationContext(createTaskDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(createTaskDto, context, validationResults, true);
        if (!isValid)
            return BadRequest(validationResults);

        var createdTask = await _taskService.CreateTaskAsync(createTaskDto);
        return CreatedAtAction(nameof(GetUserTasks), new { taskId = createdTask.Id }, createdTask);
    }

    [HttpPut("{taskId}")]
    public async Task<ActionResult<UserTaskDto>> UpdateTask(int taskId, [FromBody] UpdateUserTaskDto updateTaskDto)
    {
        // Fill the UserId and Id from the token and route parameter
        updateTaskDto.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in token"));
        updateTaskDto.Id = taskId;

        // Validate the DTO
        var context = new ValidationContext(updateTaskDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(updateTaskDto, context, validationResults, true);
        if (!isValid)
            return BadRequest(validationResults);

        // Check if the task exists and update
        var updatedTask = await _taskService.UpdateTaskAsync(updateTaskDto);
        if (updatedTask == null)
            return NotFound();

        return Ok(updatedTask);
    }
}