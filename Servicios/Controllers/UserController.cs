using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LN;
using Entities;
using Tools;

namespace Servicios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if(user == null){
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if(user == null)
            {
                return BadRequest("Datos de usuario no validos.");
            }

            var newUser = await _userService.CreateUserAsync(user);

            if(newUser == null)
            {
                return StatusCode(500, "Error al crear el usuario.");
            }

            return CreatedAtAction(nameof(GetUserById), new {id = newUser.ID}, newUser);
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User user)
        {
            if(user == null)
            {
                return BadRequest();
            }
            await _userService.PutUserByIdAsync(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _userService.DeleteUserByIdAsync(id);
            if(user == null){
                return NotFound();
            }
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var login = await _userService.LoginUser(user);

            if(login == null)
            {
                return Unauthorized();
            }

            var loginResponse = new{
                login.Nombre,
                login.Apellido,
                login.FechaNacimiento,
                login.Correo,
                login.Contraseña
            };

            return Ok(loginResponse);
        }

        [HttpPost("loginEncrypted")]
        public async Task<ActionResult<User>> LoginEncrypted(string username, string password)
        {
            try
            {
                User user = await _userService.GetUserByUsernameAsync(username);
                if(user == null){
                    return Unauthorized();
                }
                if(PasswordEncryptor.VerifyPassword(password, user.Contraseña)){
                    return user;
                }
                return Unauthorized();
            }catch(Exception ex)
            {
                return StatusCode(500, "Se ha producido un error interno del servidor: " + ex.Message);
            }
            
        }
    }
}