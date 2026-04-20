using MediRecords.Domain.Entities;
using MediRecords.Dto.PrescriptionItemDtos;

namespace MediRecords.Repository.PrescriptionItemRepository;

public interface IPrescriptionItemRepository
{
    Task<IEnumerable<PrescriptionItem>> GetAllPrescriptionItemsAsync();
    Task<PrescriptionItem?> GetPrescriptionItemByIdAsync(int itemId);
    Task<PrescriptionItem> CreatePrescriptionItemAsync(PrescriptionItem item);
    Task<PrescriptionItem> UpdatePrescriptionItemAsync(PrescriptionItem item);
    Task<bool> DeletePrescriptionItemAsync(int itemId);
    Task<bool> PrescriptionItemExistsAsync(int itemId);
    Task<bool> PrescriptionExistsAsync(int prescriptionId);
}