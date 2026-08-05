using To_do_list.Data.Entities;
using To_do_list.DTOs;

namespace To_do_list.Services;

public interface ITaskService
{
    Task<TaskDto> SaveTaskASync(CreateTaskDto taskItemDto);
    Task<List<TaskDto>> GetAllTasksAsync();
    Task<TaskItem?> GetbyIdAsync(int id);
    Task<TaskDto?> UpdateTaskASync(UpdateTaskDto taskDtoUpdate, int idTask);
    Task<bool>  DeleteTaskASync(int idTask);
    Task<bool> CompleTetaskASync(int idTask);
}