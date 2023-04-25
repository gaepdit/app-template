using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Pages.Account;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Account;

public class EditTests
{
    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        Active = true,
        Email = TestConstants.ValidEmail,
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.ValidName,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new()
    {
        Id = Guid.Empty.ToString(),
        Active = true,
    };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var pageModel = new EditModel(staffServiceMock.Object, officeServiceMock.Object, Mock.Of<IValidator<StaffUpdateDto>>())
        { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
            pageModel.OfficeItems.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");

        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        staffServiceMock.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Success);
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffServiceMock.Object, Mock.Of<IOfficeAppService>(), validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        staffServiceMock.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Failed());
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffServiceMock.Object, Mock.Of<IOfficeAppService>(), validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock.Object, officeServiceMock.Object, validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UpdateStaff.Should().Be(StaffUpdateTest);
        }
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((StaffViewDto?)null);
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(staffServiceMock.Object, Mock.Of<IOfficeAppService>(), validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }
}