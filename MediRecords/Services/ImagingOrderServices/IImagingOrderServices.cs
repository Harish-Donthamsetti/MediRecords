using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrderDto;
using MediRecords.Dto.ImagingOrderRequestDto;
namespace MediRecords.Services.ImagingOrderServices;

public interface IImagingOrderServices
{
    public Task AddAsync(ImagingOrderRequestDto dto);

    Task<List<ImagingOrder>> GetAllAsync(ImagingOrderFilterDto filter);

}
