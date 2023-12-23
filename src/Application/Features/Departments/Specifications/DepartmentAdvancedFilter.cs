namespace CleanArchitecture.Blazor.Application.Features.Departments.Specifications;
#nullable disable warnings
public enum DepartmentListView
{
    [Description("All")]
    All,
    [Description("My")]
    My,
    [Description("Created Toady")]
    CreatedToday,
    [Description("Created within the last 30 days")]
    Created30Days
}

public class DepartmentAdvancedFilter: PaginationFilter
{
    public DepartmentListView ListView { get; set; } = DepartmentListView.All;
    public UserProfile? CurrentUser { get; set; }
}