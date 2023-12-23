namespace CleanArchitecture.Blazor.Application.Features.Departments.Specifications;
#nullable disable warnings
public class DepartmentByIdSpecification : Specification<Department>
{
    public DepartmentByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}