using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CombinationPuzzle
{
    public static class Extensions
    {
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();


        // Get the display text for an enum value:
        // - Use the DisplayAttribute if set on the enum member, so this support localization
        // - Fallback on Humanizer to decamelize the enum member name
        public static string GetDisplayName<TEnum>(this TEnum value) where TEnum : Enum
        {
            // Read the Display attribute name
            var member = value.GetType().GetMember(value.ToString())[0];
            var displayAttribute = member .GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
                return displayAttribute.GetName()!;

            // Require the NuGet package Humanizer.Core
            // <PackageReference Include = "Humanizer.Core" Version = "2.8.26" />
            return value.ToString();
        }
    }
}
