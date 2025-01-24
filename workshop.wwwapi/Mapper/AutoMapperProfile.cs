using AutoMapper;

namespace workshop.wwwapi.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Models.Patient, DTO.PatientResponse>();
        CreateMap<Models.Doctor, DTO.DoctorResponse>();
        CreateMap<Models.Appointment, DTO.AppointmentResponse>()
            .ForPath(m => m.Doctor!.Appointments, opt => opt.Ignore())
            .ForPath(m => m.Patient!.Appointments, opt => opt.Ignore());
    }
}