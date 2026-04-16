using System;

namespace MediRecords.Dto.ImagingOrderRequestDto;

public class ImagingOrderRequestDto
{
    public int EncounterID { get; set; }

    public required string StudyType { get; set; }

    public required string Notes { get; set; }

}