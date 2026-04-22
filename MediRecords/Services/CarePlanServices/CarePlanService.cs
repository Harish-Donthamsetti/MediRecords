using System.Text.Json;
using MediRecords.Domain.Entities;
using MediRecords.Dto.CarePlanDtos.Request;
using MediRecords.Dto.CarePlanDtos.Response;
using MediRecords.Repository.CarePlanRepo;
using MediRecords.Utility;

namespace MediRecords.Services.CarePlanServices;

public class CarePlanService : ICarePlanService
{
    private readonly ICarePlanRepository _carePlanRepository;

    public CarePlanService(ICarePlanRepository carePlanRepository)
    {
        _carePlanRepository = carePlanRepository;
    }

    public async Task<(bool Success, string Message, CarePlanResponseDto? Data)> CreateCarePlanAsync(
        CreateCarePlanRequestDto dto)
    {
        try
        {
            // Validate PatientId
            if (dto.PatientId <= 0)
                return (false, Constant.CarePlanMessages.InvalidPatientId, null);

            // Validate Goals
            if (dto.Goals == null || dto.Goals.Count == 0 ||
                dto.Goals.All(g => string.IsNullOrWhiteSpace(g)))
                return (false, Constant.CarePlanMessages.GoalsRequired, null);

            // Validate Instructions
            if (string.IsNullOrWhiteSpace(dto.Instructions))
                return (false, Constant.CarePlanMessages.InstructionsRequired, null);

            // Check patient exists
            var patientExists = await _carePlanRepository.PatientExistsAsync(dto.PatientId);
            if (!patientExists)
                return (false, Constant.CarePlanMessages.PatientNotFound, null);

            // Serialize Goals list to JSON string for storage
            var goalsJson = JsonSerializer.Serialize(dto.Goals);

            // Create CarePlan entity
            var carePlan = new CarePlan
            {
                PatientId    = dto.PatientId,
                GoalsJSON    = goalsJson,
                Instructions = dto.Instructions,
                Status       = dto.Status
            };

            var saved = await _carePlanRepository.AddAsync(carePlan);

            // Deserialize GoalsJSON back for response
            var response = new CarePlanResponseDto
            {
                CarePlanId   = saved.CarePlanId,
                PatientId    = saved.PatientId,
                Goals        = JsonSerializer.Deserialize<List<string>>(saved.GoalsJSON) ?? new(),
                Instructions = saved.Instructions,
                Status       = saved.Status ? "Completed" : "Active"
            };

            return (true, Constant.CarePlanMessages.CarePlanCreated, response);
        }
        catch (Exception)
        {
            return (false, Constant.CarePlanMessages.SomethingWentWrong, null);
        }
    }

    
}