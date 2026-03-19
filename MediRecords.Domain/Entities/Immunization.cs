using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("Immunization")]
public class Immunization
{
    [Key]
    public int ImmunizationId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Vaccine { get; set; }

    [MaxLength(50)]
    public string Dose { get; set; }

    public DateTime GivenDate { get; set; }

    // 0: Pending, 1: Administered
    public bool Status { get; set; }

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }
}