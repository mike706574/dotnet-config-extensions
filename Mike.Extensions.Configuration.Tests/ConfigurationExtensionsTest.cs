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

        private IConfiguration BuildConfig(IDictionary<string, string> props)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(props)
                .Build();
        }
    }
}