using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;

namespace MediRecords.Repository.EncounterRepo;

public interface IEncounterRepository
{
    Task<IEnumerable<Encounter>> GetByProviderAndDateAsync(int providerId, DateTime date);
    Task<Encounter?> GetByIdAsync(int encounterId);
    Task<Encounter> UpdateStatusAsync(int encounterId, EncounterStatus newStatus);
    Task<int> GetEncounterCountAsync(DateTime fromDate, DateTime toDate);
}
