using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;

namespace MediRecords.Dto.AppointmentDtos
{
    public class AppointmentUpdateResponseDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Reason { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Message { get; set; }
    }

    public static class AppointmentUpdateResponseExtension
    {
        public static AppointmentUpdateResponseDto ToAppointmentUpdateResponse(this Appointment appointment)
        {
            return new AppointmentUpdateResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ProviderId = appointment.ProviderId,
                DateTime = appointment.DateTime,
                Reason = appointment.Reason,
                Status = appointment.Status,
                Message = "Status updated successfully"
            };
        }
    }
}