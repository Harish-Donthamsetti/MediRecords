using MediRecords.Dto.ReportsDtos;
using MediRecords.Repository.EncounterRepo;
using MediRecords.Repository.LabOrderRepository;
using MediRecords.Repository.PrescriptionRepository;
using MediRecords.Services.AuthServices;

namespace MediRecords.Services.ReportsServices;

public class ReportsService : IReportsService
{
    private readonly IEncounterRepository _encounterRepository;
    private readonly ILabOrderRepository _labOrderRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IAuthService _authService;

    public ReportsService(
        IEncounterRepository encounterRepository,
        ILabOrderRepository labOrderRepository,
        IPrescriptionRepository prescriptionRepository,
        IAuthService authService)
    {
        _encounterRepository = encounterRepository;
        _labOrderRepository = labOrderRepository;
        _prescriptionRepository = prescriptionRepository;
        _authService = authService;
    }

    public async Task<ClinicKpiReportDto> GetClinicKpiReportAsync(DateTime fromDate, DateTime toDate, int userId)
    {
        var visitCount = await _encounterRepository.GetEncounterCountAsync(fromDate, toDate);
        var labsOrdered = await _labOrderRepository.GetLabOrderCountAsync(fromDate, toDate);
        var rxIssued = await _prescriptionRepository.GetPrescriptionCountAsync(fromDate, toDate);

        // Log audit
        await _authService.SaveAuditLog(userId, $"GENERATE_CLINIC_KPIS_REPORT:{fromDate:yyyy-MM-dd}-{toDate:yyyy-MM-dd}");

        return new ClinicKpiReportDto
        {
            VisitCount = visitCount,
            LabsOrdered = labsOrdered,
            RxIssued = rxIssued
        };
    }
}