using System;
using MediRecords.Domain.Entities;
using MediRecords.Utility;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.ImagingReportRepository;

public class ImagingReportRepository : IImagingReportRepository
{
    private readonly MediRecordsDbContext _context;

    public ImagingReportRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ImagingReport report)
    {
        var orderExists = await _context.ImagingOrders
            .AnyAsync(o => o.ImagingOrderId == report.ImagingOrderId);
        if (!orderExists)
        {
            throw new KeyNotFoundException(Constant.OrderNotFound);
        }
        var reportExists = await _context.ImagingReports
            .AnyAsync(r => r.ImagingOrderId == report.ImagingOrderId);
        if (reportExists){ 
            throw new InvalidOperationException(Constant.ReportAlreadyExists);
        }
        await _context.ImagingReports.AddAsync(report);
        await _context.SaveChangesAsync();
    }
}