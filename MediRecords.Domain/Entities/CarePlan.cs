using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("CarePlan")]
public class CarePlan
{
    [Key]
    public int CarePlanId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Column(TypeName = "VARCHAR(MAX)")]
    public string GoalsJSON { get; set; }

    [Column(TypeName = "VARCHAR(MAX)")]
    public string Instructions { get; set; }

    // 0: Active, 1: Completed
    public bool Status { get; set; }

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }
}