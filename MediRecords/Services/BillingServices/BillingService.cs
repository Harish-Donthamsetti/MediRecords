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
    public async Task<(bool Success, string Message, VisitChargeResponseDto? Data, int StatusCode)>
        UpdateVisitChargeAmountAsync(int chargeId, UpdateVisitChargeRequestDto dto)
    {
        try
        {
            // Validate ChargeId
            if (chargeId <= 0)
                return (false, Constant.BillingMessages.InvalidChargeId, null, 400);

            // Validate Amount
            if (dto.Amount <= 0)
                return (false, Constant.BillingMessages.InvalidAmount, null, 400);

            // Check charge exists
            var charge = await _billingRepository.GetChargeByIdAsync(chargeId);
            if (charge == null)
                return (false, Constant.BillingMessages.ChargeNotFound, null, 404);

            // Update amount
            charge.Amount = dto.Amount;

            var updated  = await _billingRepository.UpdateAsync(charge);
            var response = _mapper.Map<VisitChargeResponseDto>(updated);

            return (true, Constant.BillingMessages.ChargeUpdated, response, 200);
        }
        catch (Exception)
        {
            return (false, Constant.BillingMessages.SomethingWentWrong, null, 500);
        }
    }
    public async Task<(bool Success, string Message, IEnumerable<VisitChargeResponseDto>? Data, int StatusCode)>
        GetChargesByEncounterIdAsync(int encounterId)
    {
        try
        {
            if (encounterId <= 0)
                return (false, Constant.BillingMessages.InvalidEncounterId, null, 400);

            var encounterExists = await _billingRepository.EncounterExistsAsync(encounterId);
            if (!encounterExists)
                return (false, Constant.BillingMessages.EncounterNotFound, null, 404);

            var charges  = await _billingRepository.GetChargesByEncounterIdAsync(encounterId);
            var response = _mapper.Map<IEnumerable<VisitChargeResponseDto>>(charges);

            return (true, string.Empty, response, 200);
        }
        catch (Exception)
        {
            return (false, Constant.BillingMessages.SomethingWentWrong, null, 500);
        }
    }
}