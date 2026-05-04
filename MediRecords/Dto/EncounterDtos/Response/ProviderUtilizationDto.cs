namespace MediRecords.Dto.EncounterDtos.Response;

public class ProviderUtilizationDto
{
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int ScheduledAppointments { get; set; }
    public int CompletedEncounters { get; set; }
    public int CancelledEncounters { get; set; }
    public double UtilizationRate { get; set; }
}