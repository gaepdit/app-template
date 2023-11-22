﻿namespace MyApp.AppServices.Staff;

/// <summary>
/// The exception that is thrown if the current user can't be found.
/// </summary>
public class CurrentUserNotFoundException : Exception
{
    public CurrentUserNotFoundException()
        : base("Information on the current user could not be found.") { }
}
