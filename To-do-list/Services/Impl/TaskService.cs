using To_do_list.Data.Entities;
using To_do_list.DTOs;
using To_do_list.Repositories;

namespace To_do_list.Services.Impl;

public class TaskService(ITaskRepository repository) : ITaskService
{
    public async Task<TaskDto> SaveTaskASync(CreateTaskDto taskItemDto)
    {
        var taskItem = new TaskItem
        {
            Title = taskItemDto.title,
            Description = taskItemDto.description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var saveTask = await repository.SaveTaskASync(taskItem);
        
        return new TaskDto(saveTask.Id, saveTask.Title, saveTask.Description, saveTask.IsCompleted, saveTask.CreatedAt);
    }

    public async Task<List<TaskDto>> GetAllTasksAsync()
    {
        var taskDb = await repository.GetAllTasksAsync();

        return taskDb.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.CreatedAt
            )).ToList();
    }

    public async Task<TaskItem?> GetbyIdAsync(int id)
    {
        return await repository.GetbyIdAsync(id);
    }

    public async Task<TaskDto?> UpdateTaskASync(UpdateTaskDto taskDtoUpdate, int idTask)
    {
        var taskItem = new TaskItem
        {
            Title = taskDtoUpdate.title,
            Description = taskDtoUpdate.description,
        };
        
        var newTask = await  repository.UpdateTaskASync(taskItem, idTask);

        if (newTask is null) return null;
        
        return new TaskDto(newTask.Id, newTask.Title, newTask.Description, newTask.IsCompleted, newTask.CreatedAt);
    }

    public async Task<bool> DeleteTaskASync(int idTask)
    {
        return await repository.DeleteTaskASync(idTask);
    }

    public async Task<bool> CompleTetaskASync(int idTask)
    {
        return await repository.CompleteTaskASync(idTask);
    }
}