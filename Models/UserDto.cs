namespace TestProjectApi.Models;

public class UserDto
{
    public int Id {get; set;}
    public string Name {get; set;}  = string.Empty;
    public string FirstName {get; set;}  = string.Empty;
    public string? FullName => $"{Name} {FirstName}";
    public string Phone {get ; set ;} = string.Empty;

    public string? Email {get; set;} = string.Empty;
    //public string Email => $"{Name}.{FirstName}@mail.com";

    public List<TaskDto>? Tasks { get; set; }

}

public class CreateUserDto
{
    public string Name {get; set;}  = string.Empty;
    public string FirstName {get; set;}  = string.Empty;
    public string? Email {get; set;} = string.Empty;
    public string Phone {get ; set ;} = string.Empty;
}