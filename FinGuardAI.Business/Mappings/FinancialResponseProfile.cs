using AutoMapper;
using FinGuardAI.DataAccess.Entities;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

public class FinancialResponseProfile : Profile
{
    public FinancialResponseProfile()
    {
      
        CreateMap<FinancialResponse, FinancialResponseDto>();

   
        CreateMap<FinancialResponseDto, FinancialResponse>();

    }
}