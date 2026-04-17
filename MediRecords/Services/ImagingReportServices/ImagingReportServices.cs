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

    public ImagingReportServices(IImagingReportRepository repo) => _repo = repo;

    public async Task CreateReportAsync(ImagingReportRequestDto dto)
    {
        if (dto.Findings == null || dto.Findings.Count == 0)
            throw new ArgumentException(Constant.InvalidFindings);

        // Serialize Dictionary to JSON String for VARCHAR(MAX) column
        string jsonFindings = JsonSerializer.Serialize(dto.Findings);

        var report = new ImagingReport
        {
            ImagingOrderId = dto.ImagingOrderID,
            Findings = jsonFindings,
            Impression = dto.Impression,
            ReportDate = DateTime.UtcNow,
            Status = true
        };

        await _repo.AddAsync(report);
    }
}
