using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mike.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration config, string key)
        {
            T value = config.GetValue<T>(key);

            if (value == null)
            {
                string message = $"No value found for required configuration property \"{key}\".";
                throw new ArgumentException(message);
            }

            return value;
        }

        public static T GetRequiredEnumValue<T>(this IConfiguration config, string key)
        {
            string value = config.GetValue<string>(key);

            if (value == null)
            {
                string message = $"No value found for required configuration property \"{key}\".";
                throw new ArgumentException(message);
            }

            try
            {
                T type = (T) Enum.Parse(typeof(T), value);
                return type;
            }
            catch (ArgumentException ex)
            {
                string options = string.Join(", ", ((T[]) Enum.GetValues(typeof(T)))
                                             .Select(option => $"\"{option}\""));

                string message = $"Invalid value \"{value}\" for configuration property \"{key}\". Valid options: {options}";
                throw new ArgumentException(message, ex);
            }
        }
    }
}