using MediRecords.Domain.Entities;
using MediRecords.Dto.LabOrderDtos;
using MediRecords.Repository.LabOrderRepository;

namespace MediRecords.Services.LabOrderServices;

public class LabOrderService : ILabOrderService
{
    private readonly ILabOrderRepository _labOrderRepository;

    public LabOrderService(ILabOrderRepository labOrderRepository)
    {
        _labOrderRepository = labOrderRepository;
    }

    public async Task<LabOrderResponseDto> CreateLabOrderAsync(LabOrderRequestDto requestDto, int providerId)
    {
        if (requestDto == null)
            throw new ArgumentNullException(nameof(requestDto));

        if (!requestDto.EncounterId.HasValue)
            throw new ArgumentException("EncounterId is required.");

        if (string.IsNullOrWhiteSpace(requestDto.TestJson))
            throw new ArgumentException("TestJson is required.");

        var labOrder = new LabOrder
        {
            EncounterId = requestDto.EncounterId.Value,
            OrderedBy = providerId,
            TestJson = requestDto.TestJson,
            OrderDate = requestDto.OrderDate ?? DateTime.UtcNow,
            Status = requestDto.Status ?? false  // false = Ordered, true = Completed
        };

        var createdLabOrder = await _labOrderRepository.CreateLabOrderAsync(labOrder);
        return LabOrderResponseDto.FromEntity(createdLabOrder);
    }

    public async Task<LabOrderResponseDto?> GetLabOrderByIdAsync(int labOrderId)
    {
        var labOrder = await _labOrderRepository.GetLabOrderByIdAsync(labOrderId);
        return labOrder != null ? LabOrderResponseDto.FromEntity(labOrder) : null;
    }

    public async Task<IEnumerable<LabOrderResponseDto>> GetLabOrdersByEncounterIdAsync(int encounterId)
    {
        var labOrders = await _labOrderRepository.GetLabOrdersByEncounterIdAsync(encounterId);
        return labOrders.Select(LabOrderResponseDto.FromEntity);
    }

    public async Task<IEnumerable<LabOrderResponseDto>> GetLabOrdersAsync(LabOrderRequestDto filter)
    {
        filter ??= new LabOrderRequestDto();
        var labOrders = await _labOrderRepository.GetLabOrdersAsync(filter);
        return labOrders.Select(LabOrderResponseDto.FromEntity);
    }
}
