using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserMapper userMapper;

        public UserApiController(IUserService userService, UserMapper userMapper)
        {
            this.userService = userService;
            this.userMapper = userMapper;
        }

        //Admin Access Only
        [HttpGet()]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<User> result = this.userService.GetAll();

            return StatusCode(StatusCodes.Status200OK, result);
        }

        //Admin - to see id
        //All - for all others
        [HttpGet("Get/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                User user = this.userService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //All users
        [HttpPost()]
        [Route("Create")]
        public IActionResult Create([FromBody] UserDTO userDTO)
        {
            try
            {
                User user = this.userMapper.Map(userDTO);
                User createdUser = this.userService.Create(user);

                return StatusCode(StatusCodes.Status201Created, user);
            }
            catch (DuplicateEntityException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
        }

        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            try
            {
                User updatedUser = this.userService.Update(id, user);

                return StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //To Create Ban User Endpoint


        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = this.userService.Delete(id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}