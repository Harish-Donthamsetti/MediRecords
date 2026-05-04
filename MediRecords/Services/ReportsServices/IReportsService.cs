using MediRecords.Dto.ReportsDtos;

namespace MediRecords.Services.ReportsServices;

public interface IReportsService
{
    Task<ClinicKpiReportDto> GetClinicKpiReportAsync(DateTime fromDate, DateTime toDate, int userId);
}