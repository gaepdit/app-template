using FluentAssertions.Execution;
using MyApp.AppServices.Offices;
using MyApp.Domain.Entities.Offices;
using MyApp.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class OfficeMapping
{
    [Test]
    public void OfficeViewMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeViewDto>(item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeUpdateMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }
}
