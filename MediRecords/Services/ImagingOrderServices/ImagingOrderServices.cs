using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrderDto;
using MediRecords.Dto.ImagingOrderRequestDto;
using MediRecords.Repository.ImagingOrderRepository;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.ImagingOrderServices;

public class ImagingOrderServices : IImagingOrderServices
{
    private readonly IImagingOrderRepository _repo;

    public ImagingOrderServices(IImagingOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task AddAsync(ImagingOrderRequestDto dto)
    {
        if (dto.EncounterID <= 0)
        {
            throw new ArgumentException(Constant.InvalidEncounterId);
        }
        var order = new ImagingOrder
        {
            EncounterId = dto.EncounterID,
            StudyType = dto.StudyType,
            Notes = dto.Notes,
            OrderedDate = DateTime.UtcNow,
            Status = true
        };
        await _repo.AddAsync(order);
    }

    public async Task<List<ImagingOrder>> GetAllAsync(ImagingOrderFilterDto filter)
    {
        return await _repo.GetAllAsync(filter);
    }
}