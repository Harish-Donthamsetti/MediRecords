using MediRecords.Domain.Entities;
using MediRecords.Dto.PrescriptionItemDtos;
using MediRecords.Repository.PrescriptionItemRepository;
using MediRecords.Utility;

namespace MediRecords.Services.PrescriptionItemServices;

public class PrescriptionItemService : IPrescriptionItemService
{
    private readonly IPrescriptionItemRepository _repository;

    public PrescriptionItemService(IPrescriptionItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PrescriptionItemResponseDto>> GetAllPrescriptionItemsAsync()
    {
        var items = await _repository.GetAllPrescriptionItemsAsync();
        return items.Select(PrescriptionItemResponseDto.FromEntity);
    }

    public async Task<PrescriptionItemResponseDto?> GetPrescriptionItemByIdAsync(int itemId)
    {
        var item = await _repository.GetPrescriptionItemByIdAsync(itemId);
        return item != null ? PrescriptionItemResponseDto.FromEntity(item) : null;
    }

    public async Task<PrescriptionItemResponseDto> CreatePrescriptionItemAsync(PrescriptionItemRequestDto request)
    {
        if (request == null)
            throw new MediRecordsException(Constant.RequestNull);

        if (!await _repository.PrescriptionExistsAsync(request.PrescriptionId))
            throw new MediRecordsException("Prescription not found.");

        var item = new PrescriptionItem
        {
            PrescriptionId = request.PrescriptionId,
            DrugName = request.DrugName,
            Dose = request.Dose,
            Frequency = request.Frequency,
            DurationDays = request.DurationDays,
            Instructions = request.Instructions
        };

        var createdItem = await _repository.CreatePrescriptionItemAsync(item);
        return PrescriptionItemResponseDto.FromEntity(createdItem);
    }

    public async Task<PrescriptionItemResponseDto> UpdatePrescriptionItemAsync(int itemId, PrescriptionItemRequestDto request)
    {
        if (request == null)
            throw new MediRecordsException(Constant.RequestNull);

        var existingItem = await _repository.GetPrescriptionItemByIdAsync(itemId);
        if (existingItem == null)
            throw new MediRecordsException("Prescription item not found.");

        if (!await _repository.PrescriptionExistsAsync(request.PrescriptionId))
            throw new MediRecordsException("Prescription not found.");

        existingItem.PrescriptionId = request.PrescriptionId;
        existingItem.DrugName = request.DrugName;
        existingItem.Dose = request.Dose;
        existingItem.Frequency = request.Frequency;
        existingItem.DurationDays = request.DurationDays;
        existingItem.Instructions = request.Instructions;

        var updatedItem = await _repository.UpdatePrescriptionItemAsync(existingItem);
        return PrescriptionItemResponseDto.FromEntity(updatedItem);
    }

    public async Task<bool> DeletePrescriptionItemAsync(int itemId)
    {
        if (!await _repository.PrescriptionItemExistsAsync(itemId))
            throw new MediRecordsException("Prescription item not found.");

        return await _repository.DeletePrescriptionItemAsync(itemId);
    }
}