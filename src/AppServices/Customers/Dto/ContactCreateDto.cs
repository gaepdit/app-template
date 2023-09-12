﻿using MyApp.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.Customers.Dto;

public record ContactCreateDto(Guid CustomerId)
{
    public string? Honorific { get; init; }

    [Display(Name = "First name")]
    public string? GivenName { get; init; }

    [Display(Name = "Last name")]
    public string? FamilyName { get; init; }

    public string? Title { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? Email { get; init; }

    public string? Notes { get; init; }

    public IncompleteAddress Address { get; init; } = new();

    public bool IsEmpty =>
        string.IsNullOrEmpty(Honorific) &&
        string.IsNullOrEmpty(GivenName) &&
        string.IsNullOrEmpty(FamilyName) &&
        string.IsNullOrEmpty(Title) &&
        string.IsNullOrEmpty(Email) &&
        string.IsNullOrEmpty(Notes) &&
        Address.IsEmpty;
}
