using MediRecords.Domain.Entities;

namespace MediRecords.Dto.PrescriptionItemDtos;

public class PrescriptionItemResponseDto
{
    public int ItemId { get; set; }
    public int PrescriptionId { get; set; }
    public string DrugName { get; set; } = null!;
    public string Dose { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int DurationDays { get; set; }
    public string Instructions { get; set; } = null!;

    public static PrescriptionItemResponseDto FromEntity(PrescriptionItem entity)
    {
        return new PrescriptionItemResponseDto
        {
            ItemId = entity.ItemId,
            PrescriptionId = entity.PrescriptionId,
            DrugName = entity.DrugName,
            Dose = entity.Dose,
            Frequency = entity.Frequency,
            DurationDays = entity.DurationDays,
            Instructions = entity.Instructions
        };
    }
}