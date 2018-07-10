using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Mike.Extensions.Configuration.Tests
{
    public class ConfigurationExtensionsTest
    {
        private enum Foo
        {
            Bar,
            Baz
        }

        [Fact]
        public void RequiredEnum()
        {
            IConfiguration config = BuildConfig(new Dictionary<string, string>
            {
                {"Foo", "Bar"}
            });
            Foo foo = config.GetRequiredEnumValue<Foo>("Foo");
            Assert.Equal(Foo.Bar, foo);
        }

        [Fact]
        public void MissingRequiredEnum()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                IConfiguration config = BuildConfig(new Dictionary<string, string>());
                config.GetRequiredEnumValue<Foo>("Foo");
            });

            Assert.Equal("No value found for required configuration property \"Foo\".",
                         exception.Message);
        }

        [Fact]
        public void InvalidRequiredEnum()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                IConfiguration config = BuildConfig(new Dictionary<string, string>
                {
                    {"Foo", "Whatever"}
                });

                config.GetRequiredEnumValue<Foo>("Foo");
            });

            Assert.Equal(
                "Invalid value \"Whatever\" for configuration property \"Foo\". Valid options: \"Bar\", \"Baz\"",
                exception.Message);
        }



        [Fact]
        public void RequiredStringValue()
        {
            IConfiguration config = BuildConfig(new Dictionary<string, string>()
            {
                {"Foo", "Bar"}
            });

            string foo = config.GetRequiredValue<string>("Foo");

            Assert.Equal("Bar", foo);
        }

        [Fact]
        public void MissingRequiredValue()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                IConfiguration config = BuildConfig(new Dictionary<string, string>());

                config.GetRequiredValue<string>("Foo");
            });

            Assert.Equal("No value found for required configuration property \"Foo\".",
                         exception.Message);
        }

        [Fact]
        public void MissingRequiredInteger()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                IConfiguration config = BuildConfig(new Dictionary<string, string>());

                config.GetRequiredValue<int>("Foo");
            });

            Assert.Equal("No value found for required configuration property \"Foo\".",
                         exception.Message);
        }

        [Fact]
        public void RequiredString()
        {
            IConfiguration config = BuildConfig(new Dictionary<string, string>()
            {
                {"Foo", "Bar"}
            });

            string foo = config.GetRequiredString("Foo");

            Assert.Equal("Bar", foo);
        }

        [Fact]
        public void NestedRequiredString()
        {
            IConfiguration config = BuildConfig(new Dictionary<string, string>()
            {
                {"Foo:Bar", "Baz"}
            });

            string foo = config.GetRequiredString("Foo:bar");

            Assert.Equal("Baz", foo);
        }

        [Fact]
        public void NestedRequiredStringFromSection()
        {
            IConfiguration config = BuildConfig(new Dictionary<string, string>()
            {
                {"Foo:Bar", "Baz"}
            });

            IConfigurationSection section = config.GetSection("Foo");

            string foo = section.GetRequiredString("Bar");

            Assert.Equal("Baz", foo);
        }

        private IConfiguration BuildConfig(IDictionary<string, string> props)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(props)
                .Build();
        }
    }
}
