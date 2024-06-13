using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeNet.Abstraction.Extension;

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
