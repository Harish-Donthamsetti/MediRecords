using System;
using System.Text.Json;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingReportDto;
using MediRecords.Repository.ImagingReportRepository;
using MediRecords.Utility;

namespace MediRecords.Services.ImagingReportServices;

public class ImagingReportServices : IImagingReportServices
{
    private readonly IImagingReportRepository _repo;

    public ImagingReportServices(IImagingReportRepository repo)
    {
        _repo = repo;
    }

    public async Task CreateReportAsync(int ImagingOrderID,ImagingReportRequestDto dto)
    {
        if (dto.Findings == null)
            throw new ArgumentException(Constant.InvalidFindings);

        string? savedFilePath = null;
        if (dto.ReportFile != null)
        {
            savedFilePath = await _repo.SaveFileAsync(dto.ReportFile);
        }

        
        string jsonFindings = JsonSerializer.Serialize(dto.Findings);

        var report = new ImagingReport
        {
            ImagingOrderId = ImagingOrderID,
            Findings = jsonFindings,
            Impression = dto.Impression,
            AttachmentPath = savedFilePath,
            ReportDate = DateTime.UtcNow,
            Status = true
        };

        await _repo.AddAsync(report);
    }
}
