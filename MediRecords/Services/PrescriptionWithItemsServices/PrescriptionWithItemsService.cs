using MediRecords.Dto.PrescriptionWithItemsDtos;
using MediRecords.Repository.PrescriptionWithItemsRepository;
using MediRecords.Utility;

namespace MediRecords.Services.PrescriptionWithItemsServices;

public class PrescriptionWithItemsService : IPrescriptionWithItemsService
{
    private readonly IPrescriptionWithItemsRepository _repository;

    public PrescriptionWithItemsService(IPrescriptionWithItemsRepository repository)
    {
        _repository = repository;
    }

    public async Task<PrescriptionWithItemsResponse> CreatePrescriptionWithItemsAsync(CreatePrescriptionWithItemsRequest request)
    {
        if (request == null)
            throw new MediRecordsException(Constant.RequestNull);

        return await _repository.CreatePrescriptionWithItemsAsync(request);
    }

    public async Task<PrescriptionWithItemsResponse?> GetPrescriptionWithItemsByIdAsync(int prescriptionId)
    {
        if (prescriptionId <= 0)
            throw new MediRecordsException("Invalid prescription ID.");

        return await _repository.GetPrescriptionWithItemsByIdAsync(prescriptionId);
    }

    public async Task<IEnumerable<PrescriptionWithItemsResponse>> GetAllPrescriptionsWithItemsAsync()
    {
        return await _repository.GetAllPrescriptionsWithItemsAsync();
    }

    public async Task<PrescriptionWithItemsResponse> UpdatePrescriptionWithItemsAsync(int prescriptionId, UpdatePrescriptionWithItemsRequest request)
    {
        if (request == null)
            throw new MediRecordsException(Constant.RequestNull);

        if (prescriptionId <= 0)
            throw new MediRecordsException("Invalid prescription ID.");

        return await _repository.UpdatePrescriptionWithItemsAsync(prescriptionId, request);
    }

    public async Task<bool> DeletePrescriptionWithItemsAsync(int prescriptionId)
    {
        if (prescriptionId <= 0)
            throw new MediRecordsException("Invalid prescription ID.");

        return await _repository.DeletePrescriptionWithItemsAsync(prescriptionId);
    }
}
