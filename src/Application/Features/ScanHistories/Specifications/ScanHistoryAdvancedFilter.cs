namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;
#nullable disable warnings
public enum ScanHistoryListView
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

public class ScanHistoryAdvancedFilter: PaginationFilter
{
    public ScanHistoryListView ListView { get; set; } = ScanHistoryListView.All;
    public UserProfile? CurrentUser { get; set; }
}