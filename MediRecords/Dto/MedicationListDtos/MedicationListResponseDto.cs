using System;
using MediRecords.Domain.Entities;

namespace MediRecords.Dto.MedicationListDtos;

public class MedicationListResponseDto
{
    public int MedId { get; set; }
    public int PatientId { get; set; }
    public string DrugName { get; set; } = null!;
    public string? Dose { get; set; }
    public string? Frequency { get; set; }
    public string? Route { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Status { get; set; }

    public static MedicationListResponseDto FromEntity(MedicationList entity)
    {
        return new MedicationListResponseDto
        {
            MedId = entity.MedId,
            PatientId = entity.PatientId,
            DrugName = entity.DrugName,
            Dose = entity.Dose,
            Frequency = entity.Frequency,
            Route = entity.Route,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status
        };
    }
}
