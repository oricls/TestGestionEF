namespace TestProjectApi.Models;

public class TaskDto
{
    public int? Id { get; set; } = 0;
    public string Name {get; set;}= string.Empty;
    public string Description {get; set;} = string.Empty;
    public bool IsCompleted {get; set;}
    public int? UserId {get; set;}
    
}

public class CreateTaskDto
{
    public string Name {get; set;}= string.Empty;
    public string? Description {get; set;}
}