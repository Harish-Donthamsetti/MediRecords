using System;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.ImagingReportRepository;

public interface IImagingReportRepository
{
    Task AddAsync(ImagingReport report);

}
