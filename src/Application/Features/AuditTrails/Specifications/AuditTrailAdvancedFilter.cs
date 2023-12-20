﻿using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.AuditTrails.Specifications;
public enum AuditTrailListView
{
    [Description("All")] All,
    [Description("My Change Histories")] My,
    [Description("Created Toady")] CreatedToday,
    [Description("View of the last 30 days")]
    Last30days
}
public class AuditTrailAdvancedFilter : PaginationFilter
{
    public AuditType? AuditType { get; set; }
    public AuditTrailListView ListView { get; set; } = AuditTrailListView.All;
    public UserProfile? CurrentUser { get; set; }
}
