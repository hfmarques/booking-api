using System.ComponentModel;

namespace Core.Domain.Enums;

public enum RoomStatusId
{
    [Description("Available")]
    Available,
    [Description("Under Maintenance")]
    UnderMaintenance
}