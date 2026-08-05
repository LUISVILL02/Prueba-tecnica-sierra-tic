namespace To_do_list.Data.Entities;

public record TaskDto(int Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt);