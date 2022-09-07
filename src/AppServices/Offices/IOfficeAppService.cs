﻿using GaEpd.Library.ListItems;
using MyAppRoot.AppServices.StaffServices;

namespace MyAppRoot.AppServices.Offices;

public interface IOfficeAppService : IDisposable
{
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default);
    Task<IReadOnlyList<StaffViewDto>> GetActiveStaffAsync(Guid id, CancellationToken token = default);
}
