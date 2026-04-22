using MediRecords.Dto.PrescriptionWithItemsDtos;

namespace MediRecords.Services.PrescriptionWithItemsServices;

public interface IPrescriptionWithItemsService
{
    Task<PrescriptionWithItemsResponse> CreatePrescriptionWithItemsAsync(CreatePrescriptionWithItemsRequest request);
    Task<PrescriptionWithItemsResponse?> GetPrescriptionWithItemsByIdAsync(int prescriptionId);
    Task<IEnumerable<PrescriptionWithItemsResponse>> GetAllPrescriptionsWithItemsAsync();
    Task<PrescriptionWithItemsResponse> UpdatePrescriptionWithItemsAsync(int prescriptionId, UpdatePrescriptionWithItemsRequest request);
    Task<bool> DeletePrescriptionWithItemsAsync(int prescriptionId);
}
