using System.Runtime.Serialization;

namespace MediRecords.Domain.Enums;

public enum UserRoleEnums
{
    [EnumMember(Value = "Admin")]
    Admin,
    [EnumMember(Value = "Physician")]
    Physician,
    [EnumMember(Value = "Nurse")]
    Nurse,
    [EnumMember(Value = "LabTechnician")]
    LabTechnician,
    [EnumMember(Value = "FrontDesk")]
    FrontDesk

    
}
