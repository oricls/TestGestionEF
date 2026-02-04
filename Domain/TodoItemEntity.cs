namespace TestProjectApi.Entities;

public class TodoItemDTO
{
    public int Id {get; set;}
    public string Name  {get; set;} = string.Empty;
    public bool IsCompleted  {get; set;}

}
