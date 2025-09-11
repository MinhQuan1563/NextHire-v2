using AutoMapper;
using NextHireApp.Dtos;
using NextHireApp.JobApplications;
using NextHireApp.Model;
using NextHireApp.UserCVs;

namespace NextHireApp;

public class NextHireAppApplicationAutoMapperProfile : Profile
{
    public NextHireAppApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<UpdateCompanyDTO, Company>()
            .ForMember(dest => dest.TaxCode, opt => opt.MapFrom(src => src.TaxCode))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry))
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))
            .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website));

        CreateMap<Company, CreateCompanyDTO>()
            .ForMember(dest => dest.TaxCode, opt => opt.MapFrom(src => src.TaxCode))
            .ForMember(dest => dest.UserCode, opt => opt.MapFrom(src => src.UserCode))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => (int)src.Industry))
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))
            .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website));

        // JobApplication mappings
        CreateMap<CreateJobApplicationDto,JobApplication>();
        CreateMap<JobApplication, JobApplicationDto>();
        CreateMap<JobApplication, JobApplicationDetailDto>();

        //UserCV
        CreateMap<UserCV, UserCvDto>();
        CreateMap<UserCV, UserCvDetailDto>();
        CreateMap<CreateUserCvDto, UserCV>();
        CreateMap<UpdateUserCvDto, UserCV>();

    }
}
