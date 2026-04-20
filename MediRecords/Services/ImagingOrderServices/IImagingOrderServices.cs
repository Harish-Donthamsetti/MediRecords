using System;
using MediRecords.Dto.ImagingOrdertDto;
namespace MediRecords.Services.ImagingOrderServices;

public interface IImagingOrderServices
{
    public Task AddAsync(int EncounterID,ImagingOrderRequestDto dto,int userId);

}
