using AutoMapper;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinGuardAI.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var usersList = await _userService.GetAll();
            if (usersList == null || !usersList.Any())
            {
                return NotFound("No users found!");
            }
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByID(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] UserAddDTO userDto)
        {
            if (userDto == null) return BadRequest("Invalid user data.");

            // 1. Check if username already exists
            if (await _userService.IsUsernameExist(userDto.Username))
            {
                return BadRequest("Username already exists, please choose another one.");
            }

            // 2. Check if the person is already linked to another user
            if (await _userService.IsPersonExist(userDto.PersonID))
            {
                return BadRequest("This person is already associated with another user account.");
            }

            var userEntity = _mapper.Map<User>(userDto);

            var result = await _userService.AddNew(userEntity);

            if (!result)
                return StatusCode(500, "A problem occurred while handling your request.");

            return CreatedAtAction(nameof(GetById), new { id = userEntity.Id }, userDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] UserDto userDto)
        {
            // 1. Ensure UserID is provided
            if (userDto.UserID <= 0)
            {
                return BadRequest("A valid UserID is required to update data.");
            }

            // 2. Find existing user in database
            var existingUser = await _userService.GetByID(userDto.UserID);

            if (existingUser == null)
            {
                return NotFound($"User with ID {userDto.UserID} not found.");
            }

            // 3. Map new data from DTO to the existing entity
            _mapper.Map(userDto, existingUser);

            // 4. Execute update
            var success = await _userService.Update(existingUser);

            if (!success)
            {
                return StatusCode(500, "An error occurred while updating the user data.");
            }

            return Ok(new { message = "Updated successfully", id = userDto.UserID });
        }

        /*
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _userService.Delete(id);
            if (!success) return NotFound();

            return Ok(new { message = "Deleted successfully", id = id });
        }
        */
    }
}