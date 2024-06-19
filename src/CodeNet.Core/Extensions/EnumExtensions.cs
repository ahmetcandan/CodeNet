using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeNet.Core.Extensions;

public static class EnumExtensions
{
    public static string? GetDisplayName(this System.Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .Name;
    }
}
