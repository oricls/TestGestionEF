namespace TestProjectApi.Entities;
public class UserEntity
{
    public int Id {get; set;}
    public string Name  {get; set;} = string.Empty;
    public string FirstName  {get; set;} = string.Empty;
    public string Email  {get; set;} = string.Empty;
    public string Phone  {get; set;} = string.Empty;
    public IList<TaskEntity> Tasks  { get; set; } = new List<TaskEntity>();  
}