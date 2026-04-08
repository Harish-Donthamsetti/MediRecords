using MediRecords.Dto.AppointmentsDtos;
namespace MediRecords.Services.AppointmentsServices;
public interface IAppointmentsService
{
    public Task<int> BookAppointmentAsync(AppointmentsRequestDto dto);
}