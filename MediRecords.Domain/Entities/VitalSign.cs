using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediRecords.Domain.Entities;

namespace MediRecords.Models;

[Table("VitalSign")]
public class VitalSign
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VitalId { get; set; } // Primary Key ID for all Vital Signs.
    
    [Required]
    [ForeignKey("EncounterIdNavigation")]
    public int EncounterId { get; set; } // ID of the Encounter.

    [Required]
    [Column(TypeName="VARCHAR(50)")]
    public string Type { get; set; } = null!; // Type of Vitals recorded.

    [Required]
    [Column(TypeName="VARCHAR(50)")]
    public string Value { get; set; } = null!; // Value of the Vitals recorded.
    
    [Required]
    [Column(TypeName="VARCHAR(20)")]
    public string Unit{ get; set; } = null!; // Unit in which the Vitals are recorded.
    
    public DateTime RecordedDate { get; set; } // Date of the Vitals recorded.

    [Required]
    [Column(TypeName="VARCHAR(50)")]
    public string RecordedBy { get; set; } = null!; // Name of the User who recorded the Vitals.

    //Foreign Key
    public virtual Encounter? EncounterIdNavigation { get; set; }

}