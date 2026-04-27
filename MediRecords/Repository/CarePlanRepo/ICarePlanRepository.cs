using System;
using MediRecords.Domain.Entities;
namespace MediRecords.Repository.CarePlanRepo;

public interface ICarePlanRepository
{
    Task<bool> PatientExistsAsync(int patientId);
    Task<CarePlan> AddAsync(CarePlan carePlan);
}
