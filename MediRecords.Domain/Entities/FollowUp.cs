using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("FollowUp")]
public class FollowUp
{
    [Key]
    public int FollowupId { get; set; }

    [Required]
    public int EncounterId { get; set; }

    public DateTime RecommendedDate { get; set; }

    [Column(TypeName = "VARCHAR(MAX)")]
    public string Notes { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Note: Assuming an Encounter entity exists to link as a Foreign Key
    // [ForeignKey("EncounterId")]
    // public virtual Encounter Encounter { get; set; }
}