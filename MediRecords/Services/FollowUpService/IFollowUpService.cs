using System;
using MediRecords.Dto.FollowUpDtos;

namespace MediRecords.Services.FollowUpService;

public interface IFollowUpService
{
    Task<int> CreateFollowUpAsync(int encounterId, FollowUpCreateRequestDto dto, int physicianId);
}
