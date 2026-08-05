using NSubstitute;
using Shouldly;
using To_do_list.Data.Entities;
using To_do_list.DTOs;
using To_do_list.Repositories;
using To_do_list.Services.Impl;
using Xunit;

namespace To_do_list.Tests.Services.Impl;

public class TaskServiceTest
{
    private readonly ITaskRepository _repository;
    private readonly TaskService _service;

    public TaskServiceTest()
    {
        _repository = Substitute.For<ITaskRepository>();
        _service = new TaskService(_repository);
    }

    [Fact]
    public async Task SaveTaskASync_WhenCalled_ReturnsTaskDtoWithMappedValues()
    {
        var dto = new CreateTaskDto("Comprar pan", "Comprar pan para el desayuno");
        var savedTask = new TaskItem
        {
            Id = 1,
            Title = dto.title,
            Description = dto.description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _repository.SaveTaskASync(Arg.Any<TaskItem>()).Returns(Task.FromResult(savedTask));

        var result = await _service.SaveTaskASync(dto);

        result.Id.ShouldBe(1);
        result.Title.ShouldBe(dto.title);
        result.Description.ShouldBe(dto.description);
        result.IsCompleted.ShouldBeFalse();
        result.CreatedAt.ShouldBe(savedTask.CreatedAt);
    }

    [Fact]
    public async Task SaveTaskASync_WhenCalled_SendsTaskItemWithCorrectValuesToRepository()
    {
        var dto = new CreateTaskDto("Tarea", "Descripción");
        TaskItem? captured = null;

        _repository.SaveTaskASync(Arg.Do<TaskItem>(x => captured = x))
            .Returns(callInfo => Task.FromResult((TaskItem)callInfo[0]!));

        var result = await _service.SaveTaskASync(dto);

        captured.ShouldNotBeNull();
        captured.Title.ShouldBe(dto.title);
        captured.Description.ShouldBe(dto.description);
        captured.IsCompleted.ShouldBeFalse();
        captured.CreatedAt.ShouldBeInRange(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

        result.Title.ShouldBe(dto.title);
        result.Description.ShouldBe(dto.description);

        await _repository.Received(1).SaveTaskASync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task GetAllTasksAsync_WhenTasksExist_ReturnsMappedTaskDtos()
    {
        var createdAt = DateTime.UtcNow;
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "T1", Description = "D1", IsCompleted = false, CreatedAt = createdAt },
            new TaskItem { Id = 2, Title = "T2", Description = "D2", IsCompleted = true, CreatedAt = createdAt }
        };

        _repository.GetAllTasksAsync().Returns(tasks);

        var result = await _service.GetAllTasksAsync();

        result.Count.ShouldBe(2);
        result[0].ShouldSatisfyAllConditions(
            x => x.Id.ShouldBe(1),
            x => x.Title.ShouldBe("T1"),
            x => x.Description.ShouldBe("D1"),
            x => x.IsCompleted.ShouldBeFalse(),
            x => x.CreatedAt.ShouldBe(createdAt));
        result[1].ShouldSatisfyAllConditions(
            x => x.Id.ShouldBe(2),
            x => x.Title.ShouldBe("T2"),
            x => x.Description.ShouldBe("D2"),
            x => x.IsCompleted.ShouldBeTrue(),
            x => x.CreatedAt.ShouldBe(createdAt));

        await _repository.Received(1).GetAllTasksAsync();
    }

    [Fact]
    public async Task GetAllTasksAsync_WhenNoTasks_ReturnsEmptyList()
    {
        _repository.GetAllTasksAsync().Returns(new List<TaskItem>());

        var result = await _service.GetAllTasksAsync();

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetbyIdAsync_WhenTaskExists_ReturnsTask()
    {
        var task = new TaskItem
        {
            Id = 5,
            Title = "T",
            Description = "D",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _repository.GetbyIdAsync(5).Returns(Task.FromResult<TaskItem?>(task));

        var result = await _service.GetbyIdAsync(5);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(5);
        result.Title.ShouldBe("T");

        await _repository.Received(1).GetbyIdAsync(5);
    }

    [Fact]
    public async Task GetbyIdAsync_WhenTaskDoesNotExist_ReturnsNull()
    {
        _repository.GetbyIdAsync(99).Returns(Task.FromResult<TaskItem?>(null));

        var result = await _service.GetbyIdAsync(99);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task UpdateTaskASync_WhenTaskExists_ReturnsUpdatedTaskDto()
    {
        var dto = new UpdateTaskDto("Nuevo título", "Nueva descripción");
        var updatedTask = new TaskItem
        {
            Id = 3,
            Title = dto.title,
            Description = dto.description,
            IsCompleted = true,
            CreatedAt = DateTime.UtcNow
        };

        _repository.UpdateTaskASync(Arg.Any<TaskItem>(), 3).Returns(Task.FromResult<TaskItem?>(updatedTask));

        var result = await _service.UpdateTaskASync(dto, 3);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(3);
        result.Title.ShouldBe(dto.title);
        result.Description.ShouldBe(dto.description);
        result.IsCompleted.ShouldBeTrue();

        await _repository.Received(1).UpdateTaskASync(Arg.Any<TaskItem>(), 3);
    }

    [Fact]
    public async Task UpdateTaskASync_WhenCalled_SendsTaskItemWithDtoValuesToRepository()
    {
        var dto = new UpdateTaskDto("Título nuevo", "Descripción nueva");
        TaskItem? captured = null;

        _repository.UpdateTaskASync(Arg.Do<TaskItem>(x => captured = x), 7)
            .Returns(callInfo => Task.FromResult<TaskItem?>(null));

        await _service.UpdateTaskASync(dto, 7);

        captured.ShouldNotBeNull();
        captured.Title.ShouldBe(dto.title);
        captured.Description.ShouldBe(dto.description);
    }

    [Fact]
    public async Task UpdateTaskASync_WhenTaskDoesNotExist_ReturnsNull()
    {
        _repository.UpdateTaskASync(Arg.Any<TaskItem>(), 999).Returns(Task.FromResult<TaskItem?>(null));

        var result = await _service.UpdateTaskASync(new UpdateTaskDto("T", "D"), 999);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteTaskASync_WhenTaskDeleted_ReturnsTrue()
    {
        _repository.DeleteTaskASync(1).Returns(true);

        var result = await _service.DeleteTaskASync(1);

        result.ShouldBeTrue();
        await _repository.Received(1).DeleteTaskASync(1);
    }

    [Fact]
    public async Task DeleteTaskASync_WhenTaskNotFound_ReturnsFalse()
    {
        _repository.DeleteTaskASync(1).Returns(false);

        var result = await _service.DeleteTaskASync(1);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task CompleteTaskASync_WhenTaskExists_ReturnsTrue()
    {
        _repository.CompleteTaskASync(1).Returns(true);

        var result = await _service.CompleTetaskASync(1);

        result.ShouldBeTrue();
        await _repository.Received(1).CompleteTaskASync(1);
    }

    [Fact]
    public async Task CompleteTaskASync_WhenTaskNotFound_ReturnsFalse()
    {
        _repository.CompleteTaskASync(1).Returns(false);

        var result = await _service.CompleTetaskASync(1);

        result.ShouldBeFalse();
    }
}
