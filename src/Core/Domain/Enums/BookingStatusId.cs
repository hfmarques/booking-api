using System.ComponentModel;

namespace Model.Enum;

public enum BookingStatusId
{
    [Description("Confirmed")]
    Confirmed = 1,
    [Description("Cancelled")]
    Cancelled
}