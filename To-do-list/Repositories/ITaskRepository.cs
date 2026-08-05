using To_do_list.Data.Entities;

namespace To_do_list.Repositories;

public interface ITaskRepository
{
    Task<TaskItem> SaveTaskASync(TaskItem taskItem);
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetbyIdAsync(int id);
    Task<TaskItem?> UpdateTaskASync(TaskItem taskItem, int idTask);
    Task<bool>  DeleteTaskASync(int idTask);
    Task<bool> CompleteTaskASync(int idTask);
}