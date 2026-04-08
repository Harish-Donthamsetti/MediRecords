using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.PatientDtos;
using MediRecords.Repositories;
using MediRecords.Repository.PatientRepo;
using MediRecords.Services.AuthServices;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.PatientServices;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IAuthService _authService;
    public PatientService(IPatientRepository patientRepo, IAuthService authService)
    {
        _patientRepo = patientRepo;
        _authService = authService;
    }

    public async Task<int> CreatePatientAsync(PatientCreateRequestDto requestDto, int frontDeskId)
    {
        if(string.IsNullOrWhiteSpace(requestDto.Name) || requestDto.DOB == default)
        {
            throw new ArgumentException("Required Field are missing");
        }

        // Generate MRN
        var mrn = await _patientRepo.GenerateMrnAsync();

        if(await _patientRepo.IsDuplicateAsync(mrn, requestDto.PhoneNo, requestDto.DOB))
        {
            throw new MediRecordsException("Patient already exists with the given details");
        }

        
        var patient = new Patient
        {
            MRN = mrn,
            Name = requestDto.Name,
            DOB = requestDto.DOB,
            Gender = requestDto.Gender,
            PhoneNo = requestDto.PhoneNo,
            AddressJSON = requestDto.AddressJSON,
            PrimaryProviderId = requestDto.PrimaryProviderId,

            CreatedBy = frontDeskId,
            CreatedAt = DateTime.UtcNow,
            Status = PatientStatus.Active
        };

        var savedPatient = await _patientRepo.AddAsync(patient);

        // Audit Log
        await _authService.SaveAuditLog(frontDeskId, $"CREATE_PATIENT:{savedPatient.PatientId}");

        return savedPatient.PatientId;
    }
}
