using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeNet.Core.Extensions;

public static class EnumExtensions
{
    public static string? GetDisplayName(this Enum enumValue)
        => enumValue
            .GetType()
            .GetMember(enumValue.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? enumValue.ToString();
}
