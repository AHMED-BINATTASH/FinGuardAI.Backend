using AutoMapper;
using FinGuardAI.API.Utilities;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinGuardAI.API.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PersonService _personService;
        private readonly IMapper _mapper;

        public PeopleController(PersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll()
        {

            var People = await _personService.GetAll();
            if (People == null || !People.Any())
            {
                return NotFound(ApiResponse<PersonDto>.FailureResponse(ResultCode.NotFound));
            }

            var PeopleDto = _mapper.Map<IEnumerable<PersonDto>>(People);

            return Ok(ApiResponse<IEnumerable<PersonDto>>.SuccessResponse(PeopleDto, ResultCode.Found));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetById(int id)
        {
            var person = await _personService.GetByID(id);
            if (person == null)
            {
                return NotFound(ApiResponse<PersonDto>.FailureResponse(ResultCode.NotFound));
            }

            var personDto = _mapper.Map<PersonDto>(person);
            return Ok(ApiResponse<PersonDto>.SuccessResponse(personDto, ResultCode.Found));
        }



        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] PersonDto personDto)
        {
            if (personDto == null) return BadRequest();


            if (await _personService.IsExistByNationalID(personDto.NationalId))
            {
                return BadRequest(ApiResponse<PersonDto>.FailureResponse(ResultCode.AlreadyExists));
            }

            var personEntity = _mapper.Map<Person>(personDto);

            var result = await _personService.AddNew(personEntity);

            if (!result)
                return StatusCode(500, ApiResponse<PersonDto>.FailureResponse(ResultCode.InternalError));

            personDto.Id = personEntity.Id;

            return CreatedAtAction(nameof(GetById), new { id = personDto.Id }, ApiResponse<PersonDto>.SuccessResponse(personDto, ResultCode.Created));
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] PersonDto personDto)
        {
            // 1. التأكد أن الـ ID موجود داخل الـ DTO المرسل
            if (personDto.Id <= 0)
            {
                return BadRequest(ApiResponse<PersonDto>.FailureResponse(ResultCode.InvalidRequest));
            }

            // 2. البحث عن الشخص في قاعدة البيانات باستخدام الـ ID الموجود في الـ DTO
            var existingPerson = await _personService.GetByID(personDto.Id);

            if (existingPerson == null)
            {
                return NotFound(ApiResponse<PersonDto>.FailureResponse(ResultCode.NotFound));
            }

            // 3. نقل البيانات من الـ DTO إلى الكائن الأصلي (Existing Entity)
            // ملاحظة: الـ AutoMapper سيقوم بتحديث الحقول الموجودة في الـ DTO فقط
            _mapper.Map(personDto, existingPerson);

            // 4. تنفيذ التحديث في قاعدة البيانات
            var success = await _personService.Update(existingPerson);

            if (!success)
            {
                return StatusCode(500, ApiResponse<PersonDto>.FailureResponse(ResultCode.InternalError));
            }

            return Ok(ApiResponse<PersonDto>.SuccessResponse(personDto, ResultCode.Updated));
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _personService.Delete(id);
            if (!success) return NotFound(ApiResponse<PersonDto>.FailureResponse(ResultCode.NotFound));

            return Ok(ApiResponse<PersonDto>.SuccessResponse(null, ResultCode.Deleted));
        }
    }
}
