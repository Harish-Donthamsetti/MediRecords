using System;

namespace MediRecords.Dto.ImagingReportDto;

public class ImagingReportRequestDto
{
    public int ImagingOrderID { get; set; }
    public Dictionary<string, string> Findings { get; set; } = new(); 
    public string Impression { get; set; } = null!;

}
