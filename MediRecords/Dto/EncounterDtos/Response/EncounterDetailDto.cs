using System;
using Humanizer;
using MediRecords.Domain.Entities;

namespace MediRecords.Dto.EncounterDtos.Response;

public class EncounterDetailDto
{
    public int EncounterId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int ProviderId { get; set; }
    public string ProivderName { get; set; } = string.Empty;
    public string VisitType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public static EncounterDetailDto FromEntity(Encounter e) => new()
    {
      EncounterId = e.EncounterId,
      PatientId = e.PatientId,
      PatientName = e.PatientIdNavigation?.Name ?? "Unknown",
      ProviderId = e.ProviderId,
      ProivderName = e.ProviderIdNavigation?.Name ?? "Unknown",
      VisitType = e.VisitType,
      Status = e.Status.ToString(),
      Date = e.Date
    };
}