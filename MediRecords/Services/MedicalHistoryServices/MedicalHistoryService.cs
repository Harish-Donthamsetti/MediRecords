using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.MedicalHistoryDtos;
using MediRecords.Repository.MedicalHistoryRepository;
using MediRecords.Repository.PatientRepo;
using MediRecords.Services.AuthServices;
using MediRecords.Utility;

namespace MediRecords.Services.MedicalHistoryServices;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IMedicalHistoryRepository _medicalHistoryRepo;
    private readonly IAuthService _authService;
    public MedicalHistoryService(IPatientRepository patientRepo, IMedicalHistoryRepository medicalHistoryRepo, IAuthService authService)
    {
        _patientRepo = patientRepo;
        _medicalHistoryRepo = medicalHistoryRepo;
        _authService = authService;
    }
    public async Task CreateMedicalHistoryAsync(int patientId, MedicalHistoryCreateRequestDto dto, int userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Condition))
            throw new MediRecordsException("Condition is required.");

        var patient = await _patientRepo.GetByIdWithDetailsAsync(patientId);
        if (patient == null)
            throw new MediRecordsException("Patient not found.");

        var history = new MedicalHistory
        {
            PatientId = patientId,
            Condition = dto.Condition.Trim(),
            Notes = dto.Notes,
            RecordedDate = DateTime.UtcNow
        };

        await _medicalHistoryRepo.AddAsync(history);

        await _authService.SaveAuditLog(
            userId,
            $"ADD_MEDICAL_HISTORY:{patientId}"
        );
    }
}
