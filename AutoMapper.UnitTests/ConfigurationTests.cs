using Api.AutoMapper;

namespace AutoMapper.UnitTests;
using AutoMapper;

public class ConfigurationTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApiProfile>();
            cfg.AddProfile<BllProfile>();
        });
        config.AssertConfigurationIsValid();
    }
}