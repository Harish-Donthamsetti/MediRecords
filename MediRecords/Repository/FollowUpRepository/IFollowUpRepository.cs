using System;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.FollowUpRepository;

public interface IFollowUpRepository
{
    Task AddFollowUpAsync(FollowUp followUp);
}
