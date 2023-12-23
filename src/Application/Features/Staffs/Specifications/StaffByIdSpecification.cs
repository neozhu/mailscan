namespace CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;
#nullable disable warnings
public class StaffByIdSpecification : Specification<Staff>
{
    public StaffByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}