using System;

using Microsoft.EntityFrameworkCore;
using MediRecords.Domain.Entities;


namespace MediRecords.Repository.CarePlanRepo;

public class CarePlanRepository : ICarePlanRepository
{
    private readonly MediRecordsDbContext _context;

    public CarePlanRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> PatientExistsAsync(int patientId)
    {
        return await _context.Patients
            .AnyAsync(p => p.PatientId == patientId);
    }

    public async Task<CarePlan> AddAsync(CarePlan carePlan)
    {
        await _context.CarePlans.AddAsync(carePlan);
        await _context.SaveChangesAsync();
        return carePlan;
    }
}
