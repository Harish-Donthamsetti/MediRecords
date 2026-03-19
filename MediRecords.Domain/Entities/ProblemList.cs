using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;


[Table("ProblemList")]
public class ProblemList
{
    [Key]
    public int ProblemId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Diagnosis { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } // Active, Resolved, Chronic

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }
}
