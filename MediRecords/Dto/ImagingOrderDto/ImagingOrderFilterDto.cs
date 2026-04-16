using System;

namespace MediRecords.Dto.ImagingOrderDto;

public class ImagingOrderFilterDto
{
    public int? ImagingOrderID { get; set; }
    public int? EncounterID { get; set; }
    public string? StudyType { get; set; }
    public string? Notes { get; set; }
    public DateTime? OrderedDate { get; set; }
    public bool? Status { get; set; }

}
