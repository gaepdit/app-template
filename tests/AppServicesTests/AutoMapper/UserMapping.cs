using FluentAssertions.Execution;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.AppServices.Staff.Dto;
using MyAppRoot.Domain.Entities.Offices;
using MyAppRoot.Domain.Identity;
using MyAppRoot.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _item = new()
    {
        Id = Guid.NewGuid().ToString(),
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.NewValidName,
        Email = TestConstants.ValidEmail,
        Phone = "123-456-7890",
        Office = new Office(Guid.NewGuid(), TestConstants.ValidName),
    };

    [Test]
    public void StaffViewMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto>(_item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(_item.Id);
            result.GivenName.Should().Be(_item.GivenName);
            result.FamilyName.Should().Be(_item.FamilyName);
            result.Email.Should().Be(_item.Email);
            result.Phone.Should().Be(_item.Phone);
            result.Office.Should().BeEquivalentTo(_item.Office);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void StaffSearchResultMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffSearchResultDto>(_item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(_item.Id);
            result.SortableFullName.Should().Be($"{_item.FamilyName}, {_item.GivenName}");
            result.Email.Should().Be(_item.Email);
            result.OfficeName.Should().Be(_item.Office!.Name);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void StaffUpdateMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffUpdateDto>(_item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(_item.Id);
            result.Phone.Should().Be(_item.Phone);
            result.OfficeId.Should().Be(_item.Office!.Id);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void NullStaffViewMappingWorks()
    {
        ApplicationUser? item = null;
        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto?>(item);
        result.Should().BeNull();
    }
}
