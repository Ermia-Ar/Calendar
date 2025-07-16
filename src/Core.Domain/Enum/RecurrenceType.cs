using System.ComponentModel;

namespace Core.Domain.Enum;

public enum RecurrenceType
{
	[Description("روز")]
	Day,
	Week,
	Month,
	Year,
}
