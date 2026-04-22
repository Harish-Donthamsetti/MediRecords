using MediRecords.Domain.Entities;
using MediRecords.Dto.PrescriptionWithItemsDtos;

namespace MediRecords.Repository.PrescriptionWithItemsRepository;

public interface IPrescriptionWithItemsRepository
{
    Task<PrescriptionWithItemsResponse> CreatePrescriptionWithItemsAsync(CreatePrescriptionWithItemsRequest request);
    Task<PrescriptionWithItemsResponse?> GetPrescriptionWithItemsByIdAsync(int prescriptionId);
    Task<IEnumerable<PrescriptionWithItemsResponse>> GetAllPrescriptionsWithItemsAsync();
    Task<PrescriptionWithItemsResponse> UpdatePrescriptionWithItemsAsync(int prescriptionId, UpdatePrescriptionWithItemsRequest request);
    Task<bool> DeletePrescriptionWithItemsAsync(int prescriptionId);
    Task<Encounter?> GetEncounterByIdAsync(int encounterId);
}
