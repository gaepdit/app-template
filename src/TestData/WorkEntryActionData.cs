﻿using MyApp.Domain.Entities.WorkEntryActions;
using MyApp.TestData.Constants;

namespace MyApp.TestData;

internal static class WorkEntryActionData
{
    private static IEnumerable<WorkEntryAction> ComplaintActionSeedItems => new List<WorkEntryAction>
    {
        new(new Guid("30000000-0000-0000-0000-000000000000"), // 0
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = $"Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
        },
        new(new Guid("30000000-0000-0000-0000-000000000001"), // 1
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = TextData.EmojiWord,
        },
        new(new Guid("30000000-0000-0000-0000-000000000002"), // 2
            WorkEntryData.GetData.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Deleted complaint action on closed complaint",
        },
        new(new Guid("30000000-0000-0000-0000-000000000003"), // 3
            WorkEntryData.GetData.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Comments = "Complaint action on a deleted complaint",
        },
        new(new Guid("30000000-0000-0000-0000-000000000004"), // 4
            WorkEntryData.GetData.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Action on current complaint",
        },
        new(new Guid("30000000-0000-0000-0000-000000000005"), // 5
            WorkEntryData.GetData.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted complaint action on current complaint",
        },
        new(new Guid("30000000-0000-0000-0000-000000000006"), // 6
            WorkEntryData.GetData.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted action on deleted complaint",
        },
    };

    private static List<WorkEntryAction>? _complaintActions;

    public static IEnumerable<WorkEntryAction> GetComplaintActions
    {
        get
        {
            if (_complaintActions is not null) return _complaintActions;

            _complaintActions = ComplaintActionSeedItems.ToList();
            _complaintActions[2].SetDeleted("00000000-0000-0000-0000-000000000001");
            _complaintActions[5].SetDeleted("00000000-0000-0000-0000-000000000001");
            _complaintActions[6].SetDeleted("00000000-0000-0000-0000-000000000001");
            return _complaintActions;
        }
    }

    public static void ClearData() => _complaintActions = null;
}
