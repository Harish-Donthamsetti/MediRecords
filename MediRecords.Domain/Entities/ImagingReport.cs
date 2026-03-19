using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediRecords.Domain.Entities;

[Table("ImagingReport")]
public class ImagingReport
{
    [Key]
    public int ReportId { get; set; }

    [Required]
    [ForeignKey("ImagingOrderIdNavigation")]
    public int ImagingOrderId { get; set; }

    [Column(TypeName = "VARCHAR(MAX)")]
    public string Findings { get; set; }

    [Column(TypeName = "VARCHAR(100)")]
    public string Impression { get; set; }

    public DateTime ReportDate { get; set; }

    public bool Status { get; set; }

    /*-------------------------Foreign Key References--------------------------------*/
    public virtual ImagingOrder? ImagingOrderIdNavigation { get; set; }
}
