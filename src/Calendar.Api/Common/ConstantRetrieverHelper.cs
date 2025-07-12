using System.Reflection;

namespace Calendar.Api.Common;

public static class ConstantRetrieverHelper
{
    public static IEnumerable<(string Name, string? Value)> GetConstants(Type type)
    {
        return (type.GetFields(BindingFlags.Static | BindingFlags.Public)).Select(x => (x.Name, x.GetRawConstantValue()?.ToString()));
    }
}