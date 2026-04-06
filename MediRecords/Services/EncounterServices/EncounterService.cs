using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Dto.EncounterDtos.Request;
using MediRecords.Dto.EncounterDtos.Response;
using MediRecords.Repository.EncounterRepo;
using MediRecords.Utility;

namespace MediRecords.Services.EncounterServices;

public class EncounterService : IEncounterService
{
    private readonly IEncounterRepository _encounterRepository;

    public EncounterService(IEncounterRepository encounterRepository)
    {
        _encounterRepository = encounterRepository;
    }
    
    public async Task<EncounterDetailDto?> GetEncounterByIdAsync(int encounterId)
    {
        if(encounterId <= 0)
            throw new MediRecordsException(Constant.EncounterMessages.InvalidEncounterId);

        var encounter = await _encounterRepository.GetByIdAsync(encounterId);

        return encounter != null ? EncounterDetailDto.FromEntity(encounter) : null;
    }

    public async Task<IEnumerable<EncounterSummaryDto>> GetWorkspaceAsync(int providerId, DateTime? date)
    {
        if(providerId <= 0)
            throw new MediRecordsException(Constant.EncounterMessages.InvalidProviderId);
        
        var targetDate = date?.Date ?? DateTime.Today;

        var encounters = await _encounterRepository.GetByProviderAndDateAsync(providerId, targetDate);
        return encounters.Select(EncounterSummaryDto.FromEntity);
    }

    public async Task<(bool Success, string Message, EncounterStatusResponseDto? Data)> UpdateEncounterStatusAsync(
        int encounterId, EncounterStatusUpdateDto dto)
    {
        if (encounterId <= 0)
            throw new MediRecordsException(Constant.EncounterMessages.InvalidEncounterId);

        // Validate that the status value is a valid enum
        if (!Enum.IsDefined(typeof(EncounterStatus), dto.Status))
            return (false, Constant.EncounterMessages.InvalidStatus, null);
        // Fetch current encounter to check if it's locked
        var existing = await _encounterRepository.GetByIdAsync(encounterId);
        if (existing == null)
            return (false, Constant.EncounterMessages.EncounterNotFound, null);

        // A locked encounter cannot be changed
        if (existing.Status == EncounterStatus.Locked)
            return (false, Constant.EncounterMessages.EncounterLocked, null);

        var updated = await _encounterRepository.UpdateStatusAsync(encounterId, dto.Status);

        var response = new EncounterStatusResponseDto
        {
            EncounterId = updated!.EncounterId,
            Status      = updated.Status.ToString(),
            Message     = Constant.EncounterMessages.StatusUpdated
        };

        return (true, Constant.EncounterMessages.StatusUpdated, response);
    }
}