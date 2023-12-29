// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Specifications;
using CleanArchitecture.Blazor.Application.Features.Departments.Queries.Pagination;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.Export;

public class ExportDepartmentsQuery : DepartmentAdvancedFilter, IRequest<Result<byte[]>>
{
    public DepartmentAdvancedSpecification Specification => new DepartmentAdvancedSpecification(this);
}

public class ExportDepartmentsQueryHandler :
         IRequestHandler<ExportDepartmentsQuery, Result<byte[]>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportDepartmentsQueryHandler> _localizer;
    private readonly DepartmentDto _dto = new();
    public ExportDepartmentsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ExportDepartmentsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Departments.ApplySpecification(request.Specification)
                   .OrderBy($"{request.OrderBy} {request.SortDirection}")
                   .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                   .AsNoTracking()
                   .ToListAsync(cancellationToken).ConfigureAwait(true);
        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<DepartmentDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.Name)],item => item.Name},
                    {_localizer[_dto.GetMemberDescription(x=>x.Address)],item => item.Address},
                    {_localizer[_dto.GetMemberDescription(x=>x.Keywords)],item => item.Keywords},
                    {_localizer[_dto.GetMemberDescription(x=>x.Description)],item => item.Description},
            }
            , _localizer[_dto.GetClassDescription()]).ConfigureAwait(false);
        return await Result<byte[]>.SuccessAsync(result).ConfigureAwait(false);
    }
}
