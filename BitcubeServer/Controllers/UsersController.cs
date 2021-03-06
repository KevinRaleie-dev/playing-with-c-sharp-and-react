using System.Collections.Generic;
using BitcubeServer.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BitcubeServer.Controllers
{
    
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() => _userService.Get();

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("register")]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);
            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(User userIn)
        {
            var user = _userService.Login(userIn);

            if (user == null)
            {
                return BadRequest(new
                {
                    message = "Invalid email or password"
                });
            }
            else {

                return Ok(new {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(User userIn, string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Update(id, userIn);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(string id)
        {
            var user = _userService.Get(id);

            if(user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);
            return NoContent();
        }

    }
}