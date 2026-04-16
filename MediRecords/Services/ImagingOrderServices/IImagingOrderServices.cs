using System;
using MediRecords.Dto.ImagingOrderRequestDto;
namespace MediRecords.Services.ImagingOrderServices;

public interface IImagingOrderServices
{
    public Task AddAsync(ImagingOrderRequestDto dto);

}
