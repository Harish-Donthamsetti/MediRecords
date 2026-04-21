using MediRecords.Dto.AppointmentsDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediRecords.Services.AppointmentsServices
{
    public interface IAppointmentsService
    {
        Task<AppointmentsResponseDto> BookAppointmentAsync(AppointmentsRequestDto dto);

        // Unified retrieval method with flexible string-based date filtering
        Task<List<AppointmentsResponseDto>> GetAppointmentsAsync(
            int? id,
            int? patientId,
            int? providerId,
            string? date);
    }
}
