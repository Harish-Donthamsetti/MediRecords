using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.PatientDtos;
using MediRecords.Repositories;
using MediRecords.Repository.PatientRepo;
using MediRecords.Repository.UserRepo;
using MediRecords.Services.AuthServices;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.PatientServices;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IUserRepository _userRepo;
    private readonly IAuthService _authService;
    public PatientService(IPatientRepository patientRepo, IUserRepository userRepository, IAuthService authService)
    {
        _patientRepo = patientRepo;
        _userRepo = userRepository;
        _authService = authService;
    }

    public async Task<int> CreatePatientAsync(PatientCreateRequestDto requestDto, int frontDeskId)
    {
        if(string.IsNullOrWhiteSpace(requestDto.Name) || requestDto.DOB == default)
        {
            throw new ArgumentException(Constant.PatientMessages.RequiredFields);
        }
  
        // Null check
        if (requestDto == null)
            throw new ArgumentException(Constant.PatientMessages.RequestNull);

        // Required field validation
        if (string.IsNullOrWhiteSpace(requestDto.Name))
            throw new ArgumentException(Constant.PatientMessages.NameRequired);

        if (string.IsNullOrWhiteSpace(requestDto.Gender))
            throw new ArgumentException(Constant.PatientMessages.GenderRequired);

        if (string.IsNullOrWhiteSpace(requestDto.PhoneNo))
            throw new ArgumentException(Constant.PatientMessages.PhoneNoRequired);

        // DOB validation (cannot be future)
        if (requestDto.DOB > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException(Constant.PatientMessages.InvalidDOB);

        // If primaryProviderId record is not present in the database
        if (requestDto.PrimaryProviderId.HasValue)
        {
            var provider =
                await _userRepo.GetUserByIdAsync(requestDto.PrimaryProviderId.Value);

            if (provider == null)
                throw new ArgumentException(
                    Constant.PatientMessages.ProviderNotFound);
            
            if (provider.RoleIdNavigation == null ||
                !string.Equals(
                    provider.RoleIdNavigation.Name,
                    Constant.Physician,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new MediRecordsException(Constant.PatientMessages.InvalidProviderId);
            }

        }

        // Generate MRN
        var mrn = await _patientRepo.GenerateMrnAsync();

        // Duplicate Check (MRN || Phone + DOB)
        var isDuplicate = await _patientRepo.IsDuplicateAsync(mrn, requestDto.PhoneNo, requestDto.DOB);
        if(isDuplicate)
        {
            throw new MediRecordsException(Constant.PatientMessages.DuplicatePatient);
        }

        
        var patient = new Patient
        {
            MRN = mrn,
            Name = requestDto.Name.Trim(),
            DOB = requestDto.DOB,
            Gender = requestDto.Gender.Trim(),
            PhoneNo = requestDto.PhoneNo.Trim(),
            ContactInfo = requestDto.ContactInfo,
            AddressJSON = requestDto.AddressJSON,
            PrimaryProviderId = requestDto.PrimaryProviderId,

            CreatedBy = frontDeskId,
            CreatedAt = DateTime.UtcNow,
            Status = PatientStatus.Active
        };

        // Persist
        var savedPatient = await _patientRepo.AddAsync(patient);

        // Audit Log
        await _authService.SaveAuditLog(frontDeskId, $"CREATE_PATIENT:{savedPatient.PatientId}");

        return savedPatient.PatientId;
    }
}
