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
        public async Task<IActionResult> PostUser(CreateUserDto user)
        {
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
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            /*
            var userEntity = await _context.Users
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity == null) return NotFound();

            return Ok(UserMapToModel(user));*/

            var param = new SqlParameter("@UserId", id);
            var result = await _context.Users
                .FromSqlRaw("EXEC sp_getUserById @UserId", param)
                .ToListAsync();
            var userEntity = result.FirstOrDefault();

            if (userEntity == null) return NotFound();

            await _context.Entry(userEntity) // Charger les tâches manuellement (car FromSqlRaw n'utilise pas Include)
                .Collection(u => u.Tasks) 
                .LoadAsync();
            
            return Ok(UserMapToDto(userEntity));
        }

        [HttpGet("user/id/{id}/tasks")]
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

        [HttpGet("user/name/{name}/tasks")]
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
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return NotFound();
            return Ok(TaskMapToDto(task));
        }

        [HttpPost("task")]
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
        public async Task<IActionResult> AssignTaskToUser(string username, string taskName)
        {
            var paramUser = new SqlParameter("@Username", username);
            var paramTask = new SqlParameter("@TaskName", taskName);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AttributeTaskToUser @Username, @TaskName",
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
