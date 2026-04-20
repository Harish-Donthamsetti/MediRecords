using MediRecords.Dto.PrescriptionItemDtos;

namespace MediRecords.Services.PrescriptionItemServices;

public interface IPrescriptionItemService
{
    Task<IEnumerable<PrescriptionItemResponseDto>> GetAllPrescriptionItemsAsync();
    Task<PrescriptionItemResponseDto?> GetPrescriptionItemByIdAsync(int itemId);
    Task<PrescriptionItemResponseDto> CreatePrescriptionItemAsync(PrescriptionItemRequestDto request);
    Task<PrescriptionItemResponseDto> UpdatePrescriptionItemAsync(int itemId, PrescriptionItemRequestDto request);
    Task<bool> DeletePrescriptionItemAsync(int itemId);
}