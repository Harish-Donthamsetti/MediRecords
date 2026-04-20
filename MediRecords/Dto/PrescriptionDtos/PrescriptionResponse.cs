using System;

namespace MediRecords.Dto.PrescriptionDtos;

public class PrescriptionResponse
{
    public int PrescriptionId { get; set; }
    public int EncounterId { get; set; }
    public int ProviderId { get; set; }
    public DateOnly CreatedDate { get; set; }
    public bool Status { get; set; }
}
