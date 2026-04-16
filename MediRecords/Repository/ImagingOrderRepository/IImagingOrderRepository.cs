using System;
using MediRecords.Domain.Entities;

namespace MediRecords.Repository.ImagingOrderRepository;

public interface IImagingOrderRepository
{
    Task AddAsync(ImagingOrder order);
}
