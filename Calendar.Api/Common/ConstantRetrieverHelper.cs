using System.Reflection;

namespace Calendar.Api.Common;

public static class ConstantRetrieverHelper
{
    public static IEnumerable<(string Name, string? Value)> GetConstants(Type type)
    {
        return ((IEnumerable<FieldInfo>)type.GetFields(BindingFlags.Static | BindingFlags.Public)).Select<FieldInfo, (string, string)>((Func<FieldInfo, (string, string)>)(x => (x.Name, x.GetRawConstantValue()?.ToString())));
    }
}