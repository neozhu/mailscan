namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;
#nullable disable warnings
public class ScanHistoryAdvancedSpecification : Specification<ScanHistory>
{
    public ScanHistoryAdvancedSpecification(ScanHistoryAdvancedFilter filter)
    {
        var today = DateTime.Now.ToUniversalTime().Date;
        var start = Convert.ToDateTime(today.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 00:00:00",
            CultureInfo.CurrentCulture);
        var end = Convert.ToDateTime(today.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 23:59:59",
            CultureInfo.CurrentCulture);
        var last30day = Convert.ToDateTime(
            today.AddDays(-30).ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 00:00:00",
            CultureInfo.CurrentCulture);

       Query.Where(q => q.MatchStatus != null)
             .Where(q => q.RecognizingText!.Contains(filter.Keyword) || q.LastName!.Contains(filter.Keyword) || q.Address!.Contains(filter.Keyword) || q.Department!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId, filter.ListView == ScanHistoryListView.My && filter.CurrentUser is not null)
             .Where(q => q.Created >= start && q.Created <= end, filter.ListView == ScanHistoryListView.CreatedToday)
             .Where(q => q.Created >= last30day, filter.ListView == ScanHistoryListView.Created30Days);
       
    }
}
