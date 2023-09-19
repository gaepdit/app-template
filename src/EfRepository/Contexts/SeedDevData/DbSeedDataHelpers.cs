﻿using Microsoft.AspNetCore.Identity;
using MyApp.Domain.Identity;
using MyApp.TestData;
using MyApp.TestData.Identity;

namespace MyApp.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedIdentityData(context);
        SeedCustomerData(context);
        SeedContactData(context);
    }

    internal static void SeedContactData(AppDbContext context)
    {
        if (context.Contacts.Any()) return;
        context.Contacts.AddRange(ContactData.GetContacts());
        context.SaveChanges();
    }

    private static void SeedCustomerData(AppDbContext context)
    {
        if (context.Customers.Any()) return;
        context.Customers.AddRange(CustomerData.GetCustomers);
        context.SaveChanges();
    }

    internal static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetOffices);
        context.SaveChanges();
    }

    internal static void SeedIdentityData(AppDbContext context)
    {
        // Seed Users
        var users = UserData.GetUsers.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = UserData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        // Seed User Roles
        if (!context.UserRoles.Any())
        {
            // -- admin
            var adminUserRoles = roles
                .Select(role => new IdentityUserRole<string>
                    { RoleId = role.Id, UserId = users.Single(e => e.GivenName == "Admin").Id })
                .ToList();
            context.UserRoles.AddRange(adminUserRoles);

            // -- staff
            var staffUserId = users.Single(e => e.GivenName == "General").Id;
            context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.SiteMaintenance).Id,
                    UserId = staffUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.Staff).Id,
                    UserId = staffUserId,
                });
        }

        context.SaveChanges();
    }
}
