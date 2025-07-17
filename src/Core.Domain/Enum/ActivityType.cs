using System.ComponentModel;

namespace Core.Domain.Enum;

public enum ActivityType
{
    [Description("تسک")]
    Task,
    [Description("رویداد")]
    Event,
    [Description("جلسه")]
    Meet
}
