using System;
using MediRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.PatientRepo;

public class PatientRepository : IPatientRepository
{
    private readonly MediRecordsDbContext _context;
    public PatientRepository(MediRecordsDbContext context)
    {
        _context = context;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

   public async Task<string> GenerateMrnAsync()
    {
        await using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT NEXT VALUE FOR PatientMrnSeq";

        if (command.Connection.State != System.Data.ConnectionState.Open)
            await command.Connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        var nextVal = Convert.ToInt64(result);

        return $"MR-{nextVal}";
    }

    public async Task<bool> IsDuplicateAsync(string mrn, string? phone, DateOnly DOB)
    {
        return await _context.Patients.AnyAsync(p => p.MRN == mrn || (!string.IsNullOrEmpty(phone) && p.PhoneNo == phone && p.DOB == DOB));
    }
}
