using System;
using MediRecords.Dto.BillingDtos.Request;
using MediRecords.Dto.BillingDtos.Response;
namespace MediRecords.Services.BillingServices;

public interface IBillingService
{
    Task<(bool Success, string Message, VisitChargeResponseDto? Data, int StatusCode)>
       AssignVisitChargeAsync(AssignVisitChargeRequestDto dto);
}
