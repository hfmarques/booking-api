using System.ComponentModel;

namespace Core.Domain.Enums;

public enum BookingStatusId
{
    [Description("Confirmed")]
    Confirmed = 1,
    [Description("Cancelled")]
    Cancelled
}