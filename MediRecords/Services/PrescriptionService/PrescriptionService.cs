using System;
using MediRecords.Dto.PrescriptionDtos;
using MediRecords.Repository.PrescriptionRepository;
using MediRecords.Utility;

namespace MediRecords.Services.PrescriptionService;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<PrescriptionResponse> CreatePrescriptionAsync(int encounterId,CreatePrescriptionRequest request)
    {
        var encounter = await _prescriptionRepository.GetEncounterByIdAsync(encounterId);
        if (encounter == null)
        {
            throw new MediRecordsException("Encounter not found.");
        }

        if(encounter.Status != Domain.Enums.EncounterStatus.Open)
        {
            throw new MediRecordsException("Encounter is closed.");
        }

        var prescriptionRequest = new CreatePrescriptionRequest
        {
            ProviderId = request.ProviderId,
            CreatedDate = request.CreatedDate,
            Status = request.Status
        };

        return await _prescriptionRepository.CreatePrescriptionAsync(encounterId, prescriptionRequest);
    }

    public Task<bool> DeletePrescriptionAsync(int prescriptionId)
    {
        return _prescriptionRepository.DeletePrescriptionAsync(prescriptionId);
    }

    public Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
    {
        return _prescriptionRepository.GetAllPrescriptionsAsync();
    }

    public Task<PrescriptionResponse> GetPrescriptionByIdAsync(int prescriptionId)
    {
        return _prescriptionRepository.GetPrescriptionByIdAsync(prescriptionId);
    }

    public Task<PrescriptionResponse> UpdatePrescriptionAsync(int prescriptionId, UpdatePrescriptionRequest request)
    {
        return _prescriptionRepository.UpdatePrescriptionAsync(prescriptionId, request);
    }
}
