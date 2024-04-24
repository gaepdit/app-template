﻿using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Notifications;
using MyApp.AppServices.UserServices;
using MyApp.AppServices.WorkEntries;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.WorkEntries;

namespace AppServicesTests.WorkEntries;

public class Find
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new WorkEntry(Guid.NewGuid());

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            Substitute.For<IEntryTypeRepository>(), Substitute.For<WorkEntryManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(),
            Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }


    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns((WorkEntry?)null);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            Substitute.For<IEntryTypeRepository>(), Substitute.For<WorkEntryManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(),
            Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.FindAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
