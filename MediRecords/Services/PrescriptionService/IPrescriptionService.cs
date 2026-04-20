using System;
using MediRecords.Dto.PrescriptionDtos;

namespace MediRecords.Services.PrescriptionService;

public interface IPrescriptionService
{
    Task<PrescriptionResponse> CreatePrescriptionAsync(int encounterId, CreatePrescriptionRequest request);
    Task<bool> DeletePrescriptionAsync(int prescriptionId);
    Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync();
    Task<PrescriptionResponse> GetPrescriptionByIdAsync(int prescriptionId);
    Task<PrescriptionResponse> UpdatePrescriptionAsync(int prescriptionId, UpdatePrescriptionRequest request);
}
