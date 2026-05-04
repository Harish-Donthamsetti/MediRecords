using System;
using MediRecords.Dto.BillingDtos.Request;
using MediRecords.Dto.BillingDtos.Response;
using AutoMapper;
using MediRecords.Domain.Entities;
using MediRecords.Repository.BillingRepo;
using MediRecords.Utility;


namespace MediRecords.Services.BillingServices;

public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;
    private readonly IMapper _mapper;

    public BillingService(IBillingRepository billingRepository, IMapper mapper)
    {
        _billingRepository = billingRepository;
        _mapper = mapper;
    }

    public async Task<(bool Success, string Message, VisitChargeResponseDto? Data, int StatusCode)> AssignVisitChargeAsync(AssignVisitChargeRequestDto dto)
    {
        try
        {
            if(dto.EncounterId <= 0)
                return (false, Constant.BillingMessages.InvalidEncounterId, null, 400);

            if(dto.CodeId <= 0)
                return (false, Constant.BillingMessages.InvalidCodeId, null, 400);
            
            var encounterExists = await _billingRepository.EncounterExistsAsync(dto.EncounterId);
            if(!encounterExists)
                return (false, Constant.BillingMessages.EncounterNotFound, null, 404);
            
            var procedureCode = await _billingRepository.GetProcedureCodeByIdAsync(dto.CodeId);
            if(procedureCode == null)
                return (false, Constant.BillingMessages.ProcedureCodeNotFound, null, 404);

            var chargeExists = await _billingRepository.ChargeExistsAsync(dto.EncounterId, dto.CodeId);
            if(chargeExists)
                return (false, Constant.BillingMessages.DuplicateCharge, null, 409);

            var amount = dto.Amount.HasValue && dto.Amount.Value > 0 ? dto.Amount.Value : procedureCode.Price;

            var charge = new VisitChargeRef
            {
                EncounterId = dto.EncounterId,
                CodeId = dto.CodeId,
                Amount = amount,
                Status = false
            };

            var saved = await _billingRepository.AddAsync(charge);
            var response = _mapper.Map<VisitChargeResponseDto>(saved);

            return (true, Constant.BillingMessages.ChargeCreated, response, 201);
        }
        catch (Exception)
        {
            return (false, Constant.BillingMessages.SomethingWentWrong, null, 500);
        }
    }
}