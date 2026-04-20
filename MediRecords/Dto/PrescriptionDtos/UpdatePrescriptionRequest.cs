using System;
using Humanizer;

namespace MediRecords.Dto.PrescriptionDtos;

public class UpdatePrescriptionRequest
{   
    public int EncounterId { get; set; }
    public int ProviderId { get; set; } 
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool Status { get; set; }
}
