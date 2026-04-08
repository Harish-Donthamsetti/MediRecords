using MediRecords.Models;
using MediRecords.Dto.VitalSignDtos;
using MediRecords.Repository.VitalSignRepository;
using MediRecords.Utility;
using MediRecords.Domain.Enums;

namespace MediRecords.Services.VitalSignServices;

public class VitalSignService : IVitalSignService
{
    private readonly IVitalSignRepository _vitalSignRepository;

    public VitalSignService(IVitalSignRepository vitalSignRepository)
    {
        _vitalSignRepository = vitalSignRepository;
    }

    public async Task<VitalSignCreateResponseDto> CreateVitalSignsAsync(VitalSignCreateRequestDto request)
    {
        var encounter = await _vitalSignRepository.GetEncounterByIdAsync(request.EncounterId);
        if (encounter == null)
        {
            throw new MediRecordsException("Encounter not found");
        }
        if (encounter.Status != EncounterStatus.Open)
        {
            throw new MediRecordsException("Encounter is closed");
        }

        VitalSign? bmiVital = null;
        double? bmi = null;
        if (request.Height.HasValue && request.Weight.HasValue)
        {
            bmi = request.Weight.Value / Math.Pow(request.Height.Value / 100, 2);
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "BMI",
                Value = bmi.Value.ToString("F2"),
                Unit = "kg/m²",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            bmiVital = await _vitalSignRepository.AddAsync(vital);
        }

        VitalSign? firstVital = null;
        if (!string.IsNullOrEmpty(request.Bp))
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "BP",
                Value = request.Bp,
                Unit = "mmHg",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }
        if (!string.IsNullOrEmpty(request.Hr))
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "HR",
                Value = request.Hr,
                Unit = "bpm",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }
        if (!string.IsNullOrEmpty(request.Temp))
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "Temp",
                Value = request.Temp,
                Unit = "°F",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }
        if (!string.IsNullOrEmpty(request.SpO2))
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "SpO2",
                Value = request.SpO2,
                Unit = "%",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }
        if (request.Height.HasValue)
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "Height",
                Value = request.Height.Value.ToString(),
                Unit = "cm",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }
        if (request.Weight.HasValue)
        {
            var vital = new VitalSign
            {
                EncounterId = request.EncounterId,
                Type = "Weight",
                Value = request.Weight.Value.ToString(),
                Unit = "kg",
                RecordedDate = DateTime.Now,
                RecordedBy = request.RecordedBy
            };
            var added = await _vitalSignRepository.AddAsync(vital);
            firstVital = firstVital ?? added;
        }

        int vitalId = bmiVital?.VitalId ?? firstVital?.VitalId ?? throw new MediRecordsException("No vitals provided");
        return new VitalSignCreateResponseDto
        {
            VitalId = vitalId,
            Bmi = bmi
        };
    }
}