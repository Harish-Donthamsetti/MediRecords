using System;
using MediRecords.Dto.ImmunizationDtos;

namespace MediRecords.Services.ImmunizationService;

public interface IImmunizationService
{
    Task<int> CreateImmunizationAsync(int patientId, ImmunizationCreateRequestDto dto, int physicianId);
}
