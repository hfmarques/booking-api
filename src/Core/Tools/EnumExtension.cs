using System.ComponentModel;

namespace Core.Tools;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        
        if (name == null) return "";
        var field = type.GetField(name);
        if (field == null) return "";

        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
        {
            return attr.Description;
        }
        
        return "";
    }
}