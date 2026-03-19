using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Xsl;

namespace MediRecords.Domain.Entities
{
    [Table("Prescription")]
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        
        [Required]
        [ForeignKey("EncounterIdNavigation")]
        public int EncounterId { get; set; }

        public int ProviderId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public bool Status { get; set; }

        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
        
        public virtual Encounter? EncounterIdNavigation { get; set; }
    }
}