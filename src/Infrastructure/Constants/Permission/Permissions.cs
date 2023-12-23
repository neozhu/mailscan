// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Reflection;

namespace CleanArchitecture.Blazor.Infrastructure.Constants.Permission;

public static class Permissions
{
    [DisplayName("AuditTrails")]
    [Description("AuditTrails Permissions")]
    public static class AuditTrails
    {
        public const string View = "Permissions.AuditTrails.View";
        public const string Search = "Permissions.AuditTrails.Search";
        public const string Export = "Permissions.AuditTrails.Export";
    }
    [DisplayName("Logs")]
    [Description("Logs Permissions")]
    public static class Logs
    {
        public const string View = "Permissions.Logs.View";
        public const string Search = "Permissions.Logs.Search";
        public const string Export = "Permissions.Logs.Export";
        public const string Purge = "Permissions.Logs.Purge";
    }


    [DisplayName("Departments")]
    [Description("Departments Permissions")]
    public static class Departments
    {
        public const string View = "Permissions.Departments.View";
        public const string Create = "Permissions.Departments.Create";
        public const string Edit = "Permissions.Departments.Edit";
        public const string Delete = "Permissions.Departments.Delete";
        public const string Search = "Permissions.Departments.Search";
        public const string Export = "Permissions.Departments.Export";
        public const string Import = "Permissions.Departments.Import";
    }
    [DisplayName("Staffs")]
    [Description("Staffs Permissions")]
    public static class Staffs
    {
        public const string View = "Permissions.Staffs.View";
        public const string Create = "Permissions.Staffs.Create";
        public const string Edit = "Permissions.Staffs.Edit";
        public const string Delete = "Permissions.Staffs.Delete";
        public const string Search = "Permissions.Staffs.Search";
        public const string Export = "Permissions.Staffs.Export";
        public const string Import = "Permissions.Staffs.Import";
    }

    [DisplayName("ScanHistories")]
    [Description("ScanHistories Permissions")]
    public static class ScanHistories
    {
        public const string View = "Permissions.ScanHistories.View";
        public const string Delete = "Permissions.ScanHistories.Delete";
        public const string Export = "Permissions.ScanHistories.Export";

    }

     
    [DisplayName("Dictionaries")]
    [Description("Dictionaries Permissions")]
    public static class Dictionaries
    {
        public const string View = "Permissions.Dictionaries.View";
        public const string Create = "Permissions.Dictionaries.Create";
        public const string Edit = "Permissions.Dictionaries.Edit";
        public const string Delete = "Permissions.Dictionaries.Delete";
        public const string Search = "Permissions.Dictionaries.Search";
        public const string Export = "Permissions.Dictionaries.Export";
        public const string Import = "Permissions.Dictionaries.Import";
    }

    [DisplayName("Users")]
    [Description("Users Permissions")]
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
        public const string Search = "Permissions.Users.Search";
        public const string Import = "Permissions.Users.Import";
        public const string Export = "Permissions.Dictionaries.Export";
        public const string ManageRoles = "Permissions.Users.ManageRoles";
        public const string RestPassword = "Permissions.Users.RestPassword";
        public const string Active = "Permissions.Users.Active";
        public const string ManagePermissions = "Permissions.Users.Permissions";
    }

    [DisplayName("Roles")]
    [Description("Roles Permissions")]
    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Edit = "Permissions.Roles.Edit";
        public const string Delete = "Permissions.Roles.Delete";
        public const string Search = "Permissions.Roles.Search";
        public const string Export = "Permissions.Roles.Export";
        public const string Import = "Permissions.Roles.Import";
        public const string ManagePermissions = "Permissions.Roles.Permissions";
        public const string ManageNavigation = "Permissions.Roles.Navigation";
    }
    [DisplayName("Multi-Tenant")]
    [Description("Multi-Tenant Permissions")]
    public static class Tenants
    {
        public const string View = "Permissions.Tenants.View";
        public const string Create = "Permissions.Tenants.Create";
        public const string Edit = "Permissions.Tenants.Edit";
        public const string Delete = "Permissions.Tenants.Delete";
        public const string Search = "Permissions.Tenants.Search";
    }
    [DisplayName("Role Claims")]
    [Description("Role Claims Permissions")]
    public static class RoleClaims
    {
        public const string View = "Permissions.RoleClaims.View";
        public const string Create = "Permissions.RoleClaims.Create";
        public const string Edit = "Permissions.RoleClaims.Edit";
        public const string Delete = "Permissions.RoleClaims.Delete";
        public const string Search = "Permissions.RoleClaims.Search";
    }

    [DisplayName("Dashboards")]
    [Description("Dashboards Permissions")]
    public static class Dashboards
    {
        public const string View = "Permissions.Dashboards.View";
    }

    [DisplayName("Hangfire")]
    [Description("Hangfire Permissions")]
    public static class Hangfire
    {
        public const string View = "Permissions.Hangfire.View";
        public const string Jobs = "Permissions.Hangfire.Jobs";
    }


    /// <summary>
    /// Returns a list of Permissions.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetRegisteredPermissions()
    {
        var permissions = new List<string>();
        foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        {
            var propertyValue = prop.GetValue(null);
            if (propertyValue is not null)
                permissions.Add((string)propertyValue);
        }
        return permissions;
    }


}
