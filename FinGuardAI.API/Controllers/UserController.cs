using AutoMapper;
using FinGuardAI.API.Utilities;
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
                return NotFound(ApiResponse<UserDto>.FailureResponse(ResultCode.NotFound));
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(usersList);

            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos, ResultCode.Found));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByID(id);
            if (user == null)
            {
                return NotFound(ApiResponse<UserDto>.FailureResponse(ResultCode.NotFound));
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto,ResultCode.Found));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] UserAddDTO userDto)
        {
            if (userDto == null) return BadRequest(ApiResponse<UserAddDTO>.FailureResponse(ResultCode.InvalidRequest));

            // 1. Check if username already exists
            if (await _userService.IsUsernameExist(userDto.Username))
            {
                return BadRequest(ApiResponse<UserAddDTO>.FailureResponse(ResultCode.AlreadyExists));
            }

            // 2. Check if the person is already linked to another user
            if (await _userService.IsPersonExist(userDto.PersonID))
            {
                return BadRequest(ApiResponse<UserAddDTO>.FailureResponse(ResultCode.AlreadyExists));
            }

            var userEntity = _mapper.Map<User>(userDto);

            var result = await _userService.AddNew(userEntity);

            if (!result)
                return StatusCode(500,ApiResponse<UserAddDTO>.FailureResponse(ResultCode.InternalError));

            return CreatedAtAction(nameof(GetById), new { id = userEntity.Id }, ApiResponse<UserAddDTO>.SuccessResponse(userDto, ResultCode.Created));
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] UserDto userDto)
        {
            // 1. Ensure UserID is provided
            if (userDto.UserID <= 0)
            {
                return BadRequest(ApiResponse<UserDto>.FailureResponse(ResultCode.BadRequest));
            }

            // 2. Find existing user in database
            var existingUser = await _userService.GetByID(userDto.UserID);

            if (existingUser == null)
            {
                return NotFound(ApiResponse<UserDto>.FailureResponse(ResultCode.NotFound));
            }

            // 3. Map new data from DTO to the existing entity
            _mapper.Map(userDto, existingUser);

            // 4. Execute update
            var success = await _userService.Update(existingUser);

            if (!success)
            {
                return StatusCode(500, ApiResponse<UserDto>.FailureResponse(ResultCode.InternalError));
            }

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, ResultCode.Updated));
        }

        
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _userService.Delete(id);
            if (!success) return NotFound(ApiResponse<UserDto>.FailureResponse(ResultCode.NotFound));

            return Ok(ApiResponse<UserDto>.SuccessResponse(null, ResultCode.Deleted));
        }
       
    }
}