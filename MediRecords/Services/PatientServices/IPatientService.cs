using System;
using MediRecords.Dto.PatientDtos;

namespace MediRecords.Services.PatientServices;

public interface IPatientService
{
    Task<int> CreatePatientAsync(PatientCreateRequestDto requestDto, int frontDeskId);
}
