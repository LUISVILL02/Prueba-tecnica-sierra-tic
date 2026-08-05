using Microsoft.EntityFrameworkCore;
using To_do_list.Data;
using To_do_list.Data.Entities;

namespace To_do_list.Repositories.Impl;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
    public async Task<TaskItem> SaveTaskASync(TaskItem taskItem)
    {
        context.Add(taskItem);
        await context.SaveChangesAsync();
        return taskItem;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        return await context.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetbyIdAsync(int id)
    {
        return  await context.TaskItems.FindAsync(id);
    }

    public async Task<TaskItem?> UpdateTaskASync(TaskItem taskItem, int idTask)
    {
        var taskFind = await context.TaskItems
            .FindAsync(idTask);

        if (taskFind is not null)
        {
            taskFind.Title = taskItem.Title;
            taskFind.Description = taskItem.Description;

            await context.SaveChangesAsync();
            
            return taskFind;
        }
        
        return null;
    }

    public async Task<bool> DeleteTaskASync(int idTask)
    {
        var taskFind = await context.TaskItems
            .FindAsync(idTask);

        if (taskFind is not null) context.TaskItems.Remove(taskFind);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CompleteTaskASync(int idTask)
    {
        var taskFind = await context.TaskItems
            .FindAsync(idTask);

        if (taskFind is not null) taskFind.IsCompleted = true;
        return await context.SaveChangesAsync() > 0;
    }
}