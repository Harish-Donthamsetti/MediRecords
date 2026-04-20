using System;
using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrdertDto;
using MediRecords.Repository.ImagingOrderRepository;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Services.ImagingOrderServices;

public class ImagingOrderServices : IImagingOrderServices
{
    private readonly IImagingOrderRepository _repo;

    public ImagingOrderServices(IImagingOrderRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Processes an imaging order request by validating the encounter ID, 
    /// mapping the DTO to a domain entity, and persisting it via the repository.
    /// </summary>
    /// <param name="dto">The data transfer object containing imaging order details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the EncounterID is less than or equal to zero.</exception>
    public async Task AddAsync(ImagingOrderRequestDto dto)
    {
        if (dto.EncounterID <= 0)
        {
            throw new ArgumentException(Constant.InvalidEncounterId);
        }
        var order = new ImagingOrder
        {
            EncounterId = dto.EncounterID,
            StudyType = dto.StudyType,
            Notes = dto.Notes,
            OrderedDate = DateTime.UtcNow,
            Status = true
        };
        await _repo.AddAsync(order);
    }
}