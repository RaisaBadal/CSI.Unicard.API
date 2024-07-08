using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.response;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSI.Unicard.API.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The service for managing users.</param>
        /// <param name="logger">The logger instance.</param>
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A task representing the asynchronous operation, with an IActionResult containing the result.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] Users user)
        {
            _logger.LogInformation("Adding user: {@User}", user);
            try
            {
                var result = await _userService.Add(user);
                _logger.LogInformation("User added successfully with result: {Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, with a Response containing the user.</returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<Response<Users>> GetById([FromRoute] int id)
        {
            _logger.LogInformation("Getting user by id: {Id}", id);
            try
            {
                var user = await _userService.GetById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with id {Id} not found", id);
                    return Response<Users>.Error($"User with id {id} not found");
                }
                _logger.LogInformation("User retrieved: {@User}", user);
                return Response<Users>.Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user.");
                return Response<Users>.Error(ErrorKeys.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a Response containing a list of users.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<Users>>> GetAll()
        {
            var res = await _userService.GetAll();
            if (!res.Any()) throw new InvalidOperationException();
            return Response<IEnumerable<Users>>.Ok(res);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpDelete]
        [Route("[action]")]
        public async Task DeleteById(int id)
        {
            await _userService.DeleteById(id);
        }
    }
}
