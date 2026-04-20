using System;
using MediRecords.Dto.PrescriptionDtos;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.PrescriptionRepository;

public interface IPrescriptionRepository
{
    Task<PrescriptionResponse> CreatePrescriptionAsync(int encounterId, CreatePrescriptionRequest request);
    Task<PrescriptionResponse> GetPrescriptionByIdAsync(int prescriptionId);
    Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync();
    Task<PrescriptionResponse> UpdatePrescriptionAsync(int prescriptionId, UpdatePrescriptionRequest request);
    Task<bool> DeletePrescriptionAsync(int prescriptionId);
    Task<Encounter?> GetEncounterByIdAsync(int encounterId);
}
