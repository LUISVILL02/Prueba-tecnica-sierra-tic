namespace To_do_list.DTOs;

public record ResponseException(int Status, string Message,  Dictionary<string,string[]> errors);