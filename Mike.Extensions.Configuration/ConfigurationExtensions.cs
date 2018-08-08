using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mike.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration config, string key)
        {
            if (config[key] == null)
            {
                string message = $"No value found for required configuration property \"{key}\".";
                throw new ArgumentException(message);
            }

            T value = config.GetValue<T>(key);

            return value;
        }

        public static T GetValueOrDefault<T>(this IConfiguration config, string key, T defaultValue)
        {
            T value = config.GetValue<T>(key);

            return value == null ? defaultValue : value;
        }

        public static string GetRequiredString(this IConfiguration config, string key)
        {
            return GetRequiredValue<string>(config, key);
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