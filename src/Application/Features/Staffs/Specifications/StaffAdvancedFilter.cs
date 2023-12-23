namespace CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;
#nullable disable warnings
public enum StaffListView
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

public class StaffAdvancedFilter: PaginationFilter
{
    public StaffListView ListView { get; set; } = StaffListView.All;
    public UserProfile? CurrentUser { get; set; }
}