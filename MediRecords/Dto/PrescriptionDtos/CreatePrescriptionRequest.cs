using System;
using DatetimeOffset = System.DateTimeOffset;

namespace MediRecords.Dto.PrescriptionDtos;

public class CreatePrescriptionRequest
{
    public int ProviderId { get; set; } 
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool Status { get; set; }

}
