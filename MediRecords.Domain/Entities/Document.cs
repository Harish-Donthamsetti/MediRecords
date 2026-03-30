using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("Document")]
public class Document
{
    [Key]
    public int DocumentID { get; set; }

    [Required]
    [ForeignKey("PatientIdNavigation")]
    public int PatientID { get; set; }

    [ForeignKey("EncounterIdNavigation")]
    public int? EncounterID { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string DocType { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string FileURI { get; set; } = string.Empty;

    [Required]
    public int UploadedBy { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime UploadedDate { get; set; }

    [Required]
    public bool Status { get; set; }

    public virtual Patient? PatientIdNavigation { get; set; }
    public virtual Encounter? EncounterIdNavigation { get; set; }
}