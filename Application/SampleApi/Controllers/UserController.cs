using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SampleApi.Contracts.Users;
using SampleApi.Mappers;
using SampleApi.Services.Users;

namespace SampleApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v1/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create the new user.
        /// </summary>
        /// <param name="userRequest">The data required for user creation.</param>
        /// <returns>The created user.</returns>
        /// <response code="201">The user was successfully created.</response>
        /// <response code="400">The create user request is malformed.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
        {
            Models.Users.User userModel = userRequest.ToDomain();
            Models.Users.User createdUser = await _userService.Create(userModel);
            User userApi = createdUser.ToApi();

            return CreatedAtRoute(Route.GetUserById, new { userId = userApi.UserId }, userApi);
        }

        /// <summary>
        /// Get a single user by unique identifier.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>The requested user.</returns>
        /// <response code="200">The user was successfully retrieved.</response>
        /// <response code="404">The requested user does not exist.</response>
        [AllowAnonymous]
        [HttpGet("{userId:guid}", Name = Route.GetUserById)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            Models.Users.User user = await _userService.Get(userId);
            User userApi = user.ToApi();

            return Ok(userApi);
        }


        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>The list of all users available.</returns>
        /// <response code="200">The users were successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {
            List<Models.Users.User> users = await _userService.GetAll();
            List<User> usersApi = users.ToApi();

            return Ok(usersApi);
        }

        /// <summary>
        /// Update the user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <param name="userRequest">The data required for user update.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">The user was successfully updated.</response>
        /// <response code="400">The user request is malformed.</response>
        /// <response code="404">The requested user does not exist.</response>
        [AllowAnonymous]
        [HttpPut("{userId:guid}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest userRequest)
        {
            Models.Users.User userUpdate = userRequest.ToDomain(userId);
            Models.Users.User updatedUser = await _userService.Update(userUpdate);

            var updatedUserApi = updatedUser.ToApi();
            return Ok(updatedUserApi);
        }

        /// <summary>
        /// Delete a single user by unique identifier.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <response code="204">The user was deleted successfully.</response>
        /// <response code="404">The requested user does not exist.</response>
        [AllowAnonymous]
        [HttpDelete("{userId:guid}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            await _userService.Delete(userId);
            return NoContent();
        }
    }
}
