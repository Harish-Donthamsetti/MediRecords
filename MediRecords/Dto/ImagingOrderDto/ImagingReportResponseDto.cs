using System;

namespace MediRecords.Dto.ImagingOrderDto;

public class ImagingReportResponseDto
{
    public int ReportId { get; set; }
    public int ImagingOrderId { get; set; }
    public Dictionary<string, string> Findings { get; set; } = new();
    public string Impression { get; set; } = null!;
    public DateTime ReportDate { get; set; }
    public bool Status { get; set; }

}
