using System;
using MediRecords.Domain.Entities;
namespace MediRecords.Repository.BillingRepo;

public interface IBillingRepository
{
    Task<bool> EncounterExistsAsync(int encounterId);
    Task<ProcedureCode?> GetProcedureCodeByIdAsync(int codeId);
    Task<bool> ChargeExistsAsync(int encounterId, int codeId);
    Task<VisitChargeRef> AddAsync(VisitChargeRef charge);
    Task<VisitChargeRef?> GetChargeByIdAsync(int chargeId);
    Task<VisitChargeRef> UpdateAsync(VisitChargeRef charge);
    Task<IEnumerable<VisitChargeRef>> GetChargesByEncounterIdAsync(int encounterId);
}