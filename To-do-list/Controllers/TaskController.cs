using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using To_do_list.Data.Entities;
using To_do_list.DTOs;
using To_do_list.Services;

namespace To_do_list.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto taskItemDto)
    {
        var result = await service.SaveTaskASync(taskItemDto);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        var result = await service.GetAllTasksAsync();
        return Ok(result);
    }

    [HttpGet("{idTask:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItem?>> GetbyIdAsync(int idTask)
    {
        var result = await service.GetbyIdAsync(idTask);
        if (result is null) return NotFound("Task not found");
        return Ok(result);
    }

    [HttpPut("{idTask:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItem?>> UpdateTask(int idTask, [FromBody] UpdateTaskDto taskDtoUpdate)
    {
        var result = await service.UpdateTaskASync(taskDtoUpdate, idTask);
        
        if (result is null) return NotFound("Task not found");
        return Ok(result);
    }

    [HttpDelete(("{idTask:int}"))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int idTask)
    {
        var result = await service.DeleteTaskASync(idTask);
        if (!result) return NotFound("Task not found");
        return Ok(result);
    }

    [HttpPatch("{idTask:int}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteTask(int idTask)
    {
        var result = await service.CompleTetaskASync(idTask);
        if (!result) return NotFound("Task not found");
        return Ok(result);
    }
}