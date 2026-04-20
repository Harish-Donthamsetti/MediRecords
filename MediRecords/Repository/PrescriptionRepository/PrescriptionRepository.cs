using System;
using MediRecords.Dto.PrescriptionDtos;
using MediRecords.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Sprache;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MediRecords.Repository.PrescriptionRepository;

public class PrescriptionRepository : IPrescriptionRepository
{

    private readonly MediRecordsDbContext _context;
    public PrescriptionRepository(MediRecordsDbContext context)
    {
        _context = context;
    }
    public async Task<PrescriptionResponse> CreatePrescriptionAsync(int encounterId, CreatePrescriptionRequest request)
    {
        if(request == null)
        {
            throw new Exception("Request cannot be null");
        }

        if(encounterId <= 0)
        {
            throw new Exception("Invalid EncounterId");
        }
        
        if(request.ProviderId <= 0)
        {
            throw new Exception("Invalid ProviderId");
        }

        var prescription = new Prescription
        {
            EncounterId = encounterId,
            ProviderId = request.ProviderId,
            CreatedDate = request.CreatedDate,
            Status = request.Status
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
        return new PrescriptionResponse
        {
            PrescriptionId = prescription.PrescriptionId,
            EncounterId = prescription.EncounterId,
            ProviderId = prescription.ProviderId,
            CreatedDate = DateOnly.FromDateTime(prescription.CreatedDate),
            Status = prescription.Status
        };
    }

    public async Task<bool> DeletePrescriptionAsync(int prescriptionId)
    {
        var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
        if(prescription == null)
        {
            throw new Exception("Prescription with the given ID does not exist");
        }
        
        try{
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            // Log the exception (ex) here as needed
            throw new Exception("An error occurred while deleting the prescription. Please try again later." + ex.Message);
        }
    }

    public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
    {
        return await _context.Prescriptions
            .Select(p => new PrescriptionResponse
            {
                PrescriptionId = p.PrescriptionId,
                EncounterId = p.EncounterId,
                ProviderId = p.ProviderId,
                CreatedDate = DateOnly.FromDateTime(p.CreatedDate),
                Status = p.Status
            })
            .ToListAsync();
    }

    public async Task<PrescriptionResponse> GetPrescriptionByIdAsync(int prescriptionId)
    {
        var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
        if (prescription == null)
        {
            throw new Exception("Prescription with the given ID does not exist");
        }

        return new PrescriptionResponse
        {
            PrescriptionId = prescription.PrescriptionId,
            EncounterId = prescription.EncounterId,
            ProviderId = prescription.ProviderId,
            CreatedDate = DateOnly.FromDateTime(prescription.CreatedDate),
            Status = prescription.Status
        };

    }
    

    public async Task<PrescriptionResponse> UpdatePrescriptionAsync(int prescriptionId, UpdatePrescriptionRequest request)
    {
        if(request == null)
        {
            throw new Exception("Request cannot be null");
        }

        if(prescriptionId <= 0)
        {
            throw new Exception("Invalid PrescriptionId");
        }

        if(request.EncounterId <= 0)
        {
            throw new Exception("Invalid EncounterId");
        }

        if(request.ProviderId <= 0)
        {
            throw new Exception("Invalid ProviderId");
        }

        var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
        if(prescription == null)
        {
            throw new Exception("Prescription with the given ID does not exist");
        }

        prescription.EncounterId = request.EncounterId;
        prescription.ProviderId = request.ProviderId;
        prescription.CreatedDate = request.CreatedDate;
        prescription.Status = request.Status;

        _context.Prescriptions.Update(prescription);
        await _context.SaveChangesAsync();

        return new PrescriptionResponse
        {
            PrescriptionId = prescription.PrescriptionId,
            EncounterId = prescription.EncounterId,
            ProviderId = prescription.ProviderId,
            CreatedDate = DateOnly.FromDateTime(prescription.CreatedDate),
            Status = prescription.Status
        };
    }

    public async Task<Encounter?> GetEncounterByIdAsync(int encounterId)
    {
        return await _context.Encounters.FindAsync(encounterId);
    }
}
