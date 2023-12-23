namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;
#nullable disable warnings
public class ScanHistoryByIdSpecification : Specification<ScanHistory>
{
    public ScanHistoryByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}