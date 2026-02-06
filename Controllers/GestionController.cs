using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestProjectApi.Entities;
using TestProjectApi.Models;

namespace TestProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionController : ControllerBase
    {
        private readonly TodoContext _context;

        public GestionController(TodoContext context)
        {
            _context = context;
        }

        [HttpPost("user")]
        [Description("Ajouter un utilisateur")]
        public async Task<IActionResult> PostUser(CreateUserDto user)
        {
            /*var paramName = new SqlParameter("@UserName", user.Name);
            var paramFirstName = new SqlParameter("@UserFirstname", user.FirstName);
            var paramEmail = new SqlParameter("@UserEmail", GenerateEmail(user.Name, user.FirstName));
            var paramPhone = new SqlParameter("@UserPhone", user.Phone);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddUser @UserName, @UserFirstname, @UserEmail, @UserPhone",
                paramName,
                paramFirstName,
                paramEmail,
                paramPhone
            );

            return NoContent(); */

            if (user == null) return BadRequest();
            var userEntity = UserMapToEntity(user);

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            
            return CreatedAtAction(
                nameof(GetUserById),
                new {id = userEntity.Id},
                UserMapToDto(userEntity)
            );
        }


        [HttpGet("user/{id}")]
        [Description("Trouver un utilisateur sur base de son ID")]
        public async Task<ActionResult<UserDto>> GetUserById(int Id)
        {
            
            /*var userEntity = await _context.Users
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity == null) return NotFound();

            return Ok(UserMapToModel(user));*/

            var param = new SqlParameter("@UserId", Id);
            var result = await _context.Users
                .FromSqlRaw("EXEC sp_getUserById @UserId", param)
                .AsNoTracking()
                .ToListAsync();
            var userEntity = result.FirstOrDefault();

            if (userEntity == null) return NotFound();

            /*await _context.Entry(userEntity)
                .Collection(u => u.Tasks) 
                .LoadAsync();*/
            
            return Ok(UserMapToDto(userEntity));
        }

        [HttpGet("user/id/{id}/tasks")]
        [Description("Trouver les tâches d'un utilisateur sur base de son ID")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByUserId(int id)
        {
            /*var tasksEntities = await _context.Tasks
                .Where(t => t.UserId  == id)
                .ToListAsync();

            if (!tasksEntities.Any())
                return NotFound($"No tasks found for user {id}");

            return Ok(tasksEntities.Select(TaskMapToModel));*/

            var param = new SqlParameter("@UserId", id);

            var tasks = await _context.Tasks
                .FromSqlRaw("EXEC sp_getTasksOfUser @UserId", param)
                .AsNoTracking()
                .ToListAsync();

            if (tasks.Count() == 0) return NotFound();

            var tasksDto = tasks.Select(TaskMapToDto).ToList();

            return Ok(tasksDto);
        }

        [HttpGet("tasks/{id}")]
        [Description("Trouver les tâches d'un utilisateur sur base de son ID")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int? id)
        {
            var tasksEntities = await _context.Tasks
                .ToListAsync();

            if (!tasksEntities.Any()) ;
                //return NotFound($"No tasks found for user {id}");

            return Ok(tasksEntities);

            //var param = new SqlParameter("@UserId", id);

            //var tasks = await _context.Tasks
            //    .FromSqlRaw("EXEC sp_getTasksOfUser @UserId", param)
            //    .AsNoTracking()
            //    .ToListAsync();

            //if (tasks.Count() == 0) return NotFound();

            //var tasksDto = tasks.Select(TaskMapToDto).ToList();

            //return Ok(tasksDto);
        }

        [HttpGet("user/name/{name}/tasks")]
        [Description("Trouver les tâches d'un utilisateur sur base de son nom")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByUserName(string name)
        {
            
            var param = new SqlParameter("@UserName", name);
            var tasks = await _context.Tasks
                .FromSqlRaw("EXEC sp_getTasksOfUsername @UserName", param)
                .AsNoTracking()
                .ToListAsync();

            if (tasks.Count() == 0) return NotFound();

            var tasksDto = tasks.Select(TaskMapToDto).ToList();

            return Ok(tasksDto);
        }
    
        [HttpGet("task/{id}")]
        [Description("Trouver une tâche sur base de son ID")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return NotFound();
            return Ok(TaskMapToDto(task));
        }

        [HttpPost("task")]
        [Description("Ajouter une novuelle tâche")]
        public async Task<IActionResult> AddNewTask(CreateTaskDto task)
        {
            var paramName = new SqlParameter("@TaskName", task.Name);
            var paramDescription = new SqlParameter("@TaskDescription", task.Description);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddTask @TaskName, @TaskDescription",
                paramName,
                paramDescription
            );

            return NoContent(); 
        }

        [HttpPut("task/assign")]
        [Description("Assigner une tâche à un utilsiateur, en founissant leur nom")]
        public async Task<IActionResult> AssignTaskToUser(string username, string taskName)
        {
            var paramUser = new SqlParameter("@Username", username);
            var paramTask = new SqlParameter("@Taskname", taskName);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AttributeTaskToUser @Username, @Taskname",
                paramUser, paramTask
            );

            return NoContent();
        }

        private static string GenerateEmail(string name, string firstName) =>  $"{name}.{firstName}@mail.com";

        private static TaskDto TaskMapToDto(TaskEntity task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                IsCompleted = task.IsCompleted,
                UserId = task.UserId
            };
        }

        private static UserDto UserMapToDto(UserEntity user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                FirstName = user.FirstName,
                Email = user.Email,
                Phone = user.Phone,
                Tasks = user.Tasks?
                    .Select(TaskMapToDto)
                    .ToList()
            };
        }

        private static UserEntity UserMapToEntity(CreateUserDto user){
            return new UserEntity
            {
                Name = user.Name,
                FirstName = user.FirstName,
                Phone = user.Phone,
                Email = GenerateEmail(user.Name, user.FirstName)
            };
        }
    }
}
