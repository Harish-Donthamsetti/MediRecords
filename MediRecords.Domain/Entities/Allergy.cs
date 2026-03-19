using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("Allergy")]
public class Allergy
{
    [Key]
    public int AllergyId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Allergen { get; set; }

    public string? Reaction { get; set; }

    [MaxLength(50)]
    public string? Severity { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";

    public DateTime NotedDate { get; set; } = DateTime.Now;

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }
}
