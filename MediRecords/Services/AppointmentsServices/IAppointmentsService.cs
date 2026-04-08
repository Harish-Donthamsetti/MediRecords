using MediRecords.Dto.AppointmentsDtos;
using System.Threading.Tasks;

namespace MediRecords.Services.AppointmentsServices
{
    public interface IAppointmentsService
    {
        Task<AppointmentsResponseDto> BookAppointmentAsync(AppointmentsRequestDto dto);
    }
}
