using AutoMapper;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.Business.Mappings
{

    public class FinancialRequestProfile : Profile
    {

        public FinancialRequestProfile()
        {
            // Mapping from Entity to DTO
            CreateMap<FinancialRequest, FinancialRequestDto>()
                .ForMember(dest => dest.RequestCategory,
                           opt => opt.MapFrom(src => src.RequestCategory.ToString()));

            // Mapping from DTO to Entity (Optional, if you need to save data)
            CreateMap<FinancialRequestDto, FinancialRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestCategory,
                           opt => opt.MapFrom(src => Enum.Parse<FinancialRequest.Categories>(src.RequestCategory)));


        }
    }
}


