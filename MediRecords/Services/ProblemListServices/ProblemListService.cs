using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.ProblemListDtos;
using MediRecords.Repositories;
using MediRecords.Repository.PatientRepo;
using MediRecords.Repository.ProblemListRepository;
using MediRecords.Services.AuthServices;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.ProblemListServices;

public class ProblemListService : IProblemListService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IProblemListRepository _problemListRepo;
    private readonly IAuthService _authService;
    public ProblemListService(IPatientRepository patientRepo, IProblemListRepository problemListRepo, IAuthService authService)
    {
        _patientRepo = patientRepo;
        _problemListRepo = problemListRepo;
        _authService = authService;
    }

    public async Task CreateProblemAsync(int patientId, ProblemCreateRequestDto dto, int userId)
    {
        
        if (string.IsNullOrWhiteSpace(dto.Diagnosis))
            throw new MediRecordsException("Diagnosis is required.");

        if (dto.StartDate > DateTime.UtcNow)
            throw new MediRecordsException("Start date cannot be in the future.");

        var patient = await _patientRepo.GetByIdWithDetailsAsync(patientId);
        if (patient == null)
            throw new MediRecordsException("Patient not found.");

        var problem = new ProblemList
        {
            PatientId = patientId,
            Diagnosis = dto.Diagnosis.Trim(),
            Status = ProblemStatus.Active,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        await _problemListRepo.AddAsync(problem);

        await _authService.SaveAuditLog(
            userId,
            $"ADD_PROBLEM:{patientId}"
        );
    }
}
