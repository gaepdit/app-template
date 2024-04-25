﻿using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.UserServices;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<EntryType> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IEntryTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}