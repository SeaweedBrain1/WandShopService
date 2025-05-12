using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WandShop.Infrastructure.Converters;

public static class StringDisplayConverter
{
    public static string ConvertToPrettyCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var result = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
        result = Regex.Replace(result, "([A-Z])([A-Z])", "$1 $2");

        return result;
    }
}
