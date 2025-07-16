using System.ComponentModel;

namespace Core.Domain.Enum;

public enum ActivityCategory
{
    [Description("تسک")]
    Task,
    [Description("رویداد")]
    Event,
}
