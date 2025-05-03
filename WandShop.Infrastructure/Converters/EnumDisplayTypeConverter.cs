using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WandShop.Infrastructure.Converters;

/*
https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-9.0
https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.typeconverter?view=net-9.0
-- parsing data from queries
*/

public class EnumDisplayTypeConverter<T> : TypeConverter where T : struct, Enum
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string);

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        var input = value?.ToString();
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Value cannot be null or whitespace.");

        // Usuwa spacje i porównuje z nazwami enumów
        var normalized = Regex.Replace(input, @"\s+", "");

        if (Enum.TryParse<T>(normalized, ignoreCase: true, out var result))
            return result;

        throw new ArgumentException($"Cannot convert '{input}' to {typeof(T).Name}");
    }
}
