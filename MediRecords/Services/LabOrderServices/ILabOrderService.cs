using MediRecords.Dto.LabOrderDtos;

namespace MediRecords.Services.LabOrderServices;

public interface ILabOrderService
{
    Task<LabOrderResponseDto> CreateLabOrderAsync(LabOrderRequestDto requestDto, int providerId);
    Task<LabOrderResponseDto?> GetLabOrderByIdAsync(int labOrderId);
    Task<IEnumerable<LabOrderResponseDto>> GetLabOrdersByEncounterIdAsync(int encounterId);
    Task<IEnumerable<LabOrderResponseDto>> GetLabOrdersAsync(LabOrderRequestDto filter);
}
