// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
using CleanArchitecture.Blazor.Application.Features.Departments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.Pagination;

public class DepartmentsWithPaginationQuery : DepartmentAdvancedFilter, ICacheableRequest<PaginatedData<DepartmentDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => DepartmentCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => DepartmentCacheKey.MemoryCacheEntryOptions;
    public DepartmentAdvancedSpecification Specification => new DepartmentAdvancedSpecification(this);
}
    
public class DepartmentsWithPaginationQueryHandler :
         IRequestHandler<DepartmentsWithPaginationQuery, PaginatedData<DepartmentDto>>
{
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DepartmentsWithPaginationQueryHandler> _localizer;

        public DepartmentsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<DepartmentsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<DepartmentDto>> Handle(DepartmentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.Departments.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                    .ProjectToPaginatedDataAsync<Department, DepartmentDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken).ConfigureAwait(false);
            return data;
        }
}
