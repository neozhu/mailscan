﻿namespace CleanArchitecture.Blazor.Application.Features.Loggers.Specifications;
public enum LogListView
{
    [Description("All")] All,
    [Description("Created Toady")] CreatedToday,
    [Description("View of the last 30 days")]
    Last30days
}

public class LoggerAdvancedFilter : PaginationFilter
{
    public LogLevel? Level { get; set; }
    public LogListView ListView { get; set; } = LogListView.All;
}
