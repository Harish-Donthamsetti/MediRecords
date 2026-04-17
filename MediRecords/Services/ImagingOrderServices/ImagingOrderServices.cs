using System;
using System.Text.Json;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrderDto;
using MediRecords.Repository.ImagingOrderRepository;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.ImagingOrderServices;

public class ImagingOrderServices : IImagingOrderServices
{
    private readonly IImagingOrderRepository _repo;

    public ImagingOrderServices(IImagingOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task AddAsync(ImagingOrderRequestDto dto)
    {
        if (dto.EncounterID <= 0)
        {
            throw new ArgumentException(Constant.InvalidEncounterId);
        }
        var order = new ImagingOrder
        {
            EncounterId = dto.EncounterID,
            StudyType = dto.StudyType,
            Notes = dto.Notes,
            OrderedDate = DateTime.UtcNow,
            Status = true
        };
        await _repo.AddAsync(order);
    }

    public async Task<List<ImagingOrderResponseDto>> GetAllAsync(ImagingOrderFilterDto filter)
    {
        var orders = await _repo.GetAllAsync(filter);
        return orders.Select(o => new ImagingOrderResponseDto
        {
            ImagingOrderId = o.ImagingOrderId,
            EncounterId = o.EncounterId,
            StudyType = o.StudyType,
            Notes = o.Notes,
            OrderedDate = o.OrderedDate,
            Status = o.Status,
            // Map the reports and unpack the Findings
            Reports = o.ImagingReports.Select(r => new ImagingReportResponseDto
            {
                ReportId = r.ReportId,
                ImagingOrderId = r.ImagingOrderId,
                Impression = r.Impression,
                ReportDate = r.ReportDate,
                Status = r.Status,
                Findings = string.IsNullOrWhiteSpace(r.Findings)
                    ? new Dictionary<string, string>()
                    : JsonSerializer.Deserialize<Dictionary<string, string>>(r.Findings) ?? new()
            }).ToList()
        }).ToList();
    }
}