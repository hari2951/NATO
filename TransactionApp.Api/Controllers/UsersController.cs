using Microsoft.AspNetCore.Mvc;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            logger.LogInformation(LogMessages.CreatingUser, dto.FullName);

            var created = await userService.CreateAsync(dto);

            logger.LogInformation(LogMessages.UserCreated, created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            logger.LogInformation(LogMessages.FetchingUser, id);

            var user = await userService.GetByIdAsync(id);
            if (user == null)
            {
                logger.LogWarning(LogMessages.UserNotFound, id);
                return NotFound();
            }

            logger.LogInformation(LogMessages.UserFound, id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateUserDto dto)
        {
            logger.LogInformation(LogMessages.UpdatingUser, id);

            var updated = await userService.UpdateAsync(id, dto);
            if (!updated)
            {
                logger.LogWarning(LogMessages.UserNotFound, id);
                return NotFound();
            }

            logger.LogInformation(LogMessages.UserUpdated, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            logger.LogInformation(LogMessages.DeletingUser, id);

            var deleted = await userService.DeleteAsync(id);
            if (!deleted)
            {
                logger.LogWarning(LogMessages.UserNotFound, id);
                return NotFound();
            }

            logger.LogInformation(LogMessages.UserDeleted, id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation(LogMessages.FetchingAllUsers);

            var users = await userService.GetAllAsync();
            return Ok(users);
        }
    }
}