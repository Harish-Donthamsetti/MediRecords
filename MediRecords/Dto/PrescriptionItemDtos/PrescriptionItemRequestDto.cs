using System.ComponentModel.DataAnnotations;

namespace MediRecords.Dto.PrescriptionItemDtos;

public class PrescriptionItemRequestDto
{
    public int? ItemId { get; set; } // For update, optional

    [Required]
    public int PrescriptionId { get; set; }

    [Required]
    [StringLength(100)]
    public string DrugName { get; set; } = null!;

    [StringLength(50)]
    public string Dose { get; set; } = null!;

    [StringLength(50)]
    public string Frequency { get; set; } = null!;

    public int DurationDays { get; set; }

    public string Instructions { get; set; } = null!;
}