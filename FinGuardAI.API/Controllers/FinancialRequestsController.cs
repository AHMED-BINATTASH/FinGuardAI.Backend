using AutoMapper;
using FinGuardAI.API.Utilities;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Parameters;
using Microsoft.AspNetCore.Mvc;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.API.Controllers
{
    [Route("api/FinancialRequests")]
    [ApiController]
    public class FinancialRequestsController : ControllerBase
    {
        private readonly FinancialRequestService _financialRequestService;
        private readonly IMapper _mapper;

        public FinancialRequestsController(FinancialRequestService financialRequestService, IMapper mapper)
        {
            _financialRequestService = financialRequestService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FinancialRequestDto>>> GetAll()
        {
            var requests = await _financialRequestService.GetAll();

            if (requests == null || !requests.Any())
            {
                return NotFound(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.NotFound));
            }

            var requestsDto = _mapper.Map<IEnumerable<FinancialRequestDto>>(requests);

            return Ok(ApiResponse<IEnumerable<FinancialRequestDto>>.SuccessResponse(requestsDto, ResultCode.Found));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialRequestDto>> GetById(int id)
        {
            var request = await _financialRequestService.GetByID(id);

            if (request == null)
            {
                return NotFound(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.NotFound));
            }

            var requestDto = _mapper.Map<FinancialRequestDto>(request);

            return Ok(ApiResponse<FinancialRequestDto>.SuccessResponse(requestDto, ResultCode.Found));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] FinancialRequestDto requestDto)
        {
            if (requestDto == null) return BadRequest();

            // تحويل الـ DTO إلى Entity
            var requestEntity = _mapper.Map<FinancialRequest>(requestDto);


            requestEntity.CreatedAt = DateTime.Now;

            var result = await _financialRequestService.AddNew(requestEntity);

            if (!result)
                return StatusCode(500, ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.InternalError));

            return CreatedAtAction(
                nameof(GetById),
                new { id = requestEntity.Id },
                ApiResponse<FinancialRequestDto>.SuccessResponse(requestDto, ResultCode.Created));
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] FinancialRequestDto requestDto)
        {
            if (requestDto.Id <= 0)
            {
                return BadRequest(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.BadRequest));
            }

            var existingRequest = await _financialRequestService.GetByID(requestDto.Id);

            if (existingRequest == null)
            {
                return NotFound(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.NotFound));
            }

            // نقل البيانات من الـ DTO إلى الـ Entity
            _mapper.Map(requestDto, existingRequest);

            var success = await _financialRequestService.Update(existingRequest);

            if (!success)
            {
                return StatusCode(500, ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.InternalError));
            }

            return Ok(ApiResponse<FinancialRequestDto>.SuccessResponse(requestDto, ResultCode.Updated));
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _financialRequestService.Delete(id);

            if (!success) return NotFound(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.NotFound));

            return Ok(ApiResponse<FinancialRequestDto>.SuccessResponse(null, ResultCode.Deleted));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<FinancialRequestDto>>> GetByFilter([FromQuery] RequestFilterParameters filterParams)
        {
            var filteredRequests = await _financialRequestService.GetByFilter(filterParams);
            if (filteredRequests == null || !filteredRequests.Any())
            {
                return NotFound(ApiResponse<FinancialRequestDto>.FailureResponse(ResultCode.NotFound));
            }
            var filteredRequestsDto = _mapper.Map<IEnumerable<FinancialRequestDto>>(filteredRequests);
            return Ok(ApiResponse<IEnumerable<FinancialRequestDto>>.SuccessResponse(filteredRequestsDto, ResultCode.Found));
        }
    }
}