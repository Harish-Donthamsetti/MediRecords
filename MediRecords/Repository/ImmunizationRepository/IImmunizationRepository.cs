using System;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.ImmunizationRepository;

public interface IImmunizationRepository
{
    Task AddAsync(Immunization immunization);
}
