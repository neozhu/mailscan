namespace CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;
#nullable disable warnings
public class StaffAdvancedSpecification : Specification<Staff>
{
    public StaffAdvancedSpecification(StaffAdvancedFilter filter)
    {
        var today = DateTime.Now.ToUniversalTime().Date;
        var start = Convert.ToDateTime(today.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 00:00:00",
            CultureInfo.CurrentCulture);
        var end = Convert.ToDateTime(today.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 23:59:59",
            CultureInfo.CurrentCulture);
        var last30day = Convert.ToDateTime(
            today.AddDays(-30).ToString("yyyy-MM-dd", CultureInfo.CurrentCulture) + " 00:00:00",
            CultureInfo.CurrentCulture);

       Query.Where(q => q.LastName != null)
             .Where(q => q.LastName!.Contains(filter.Keyword) || q.FirstName!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId, filter.ListView == StaffListView.My && filter.CurrentUser is not null)
             .Where(q => q.Created >= start && q.Created <= end, filter.ListView == StaffListView.CreatedToday)
             .Where(q => q.Created >= last30day, filter.ListView == StaffListView.Created30Days);
       
    }
}
