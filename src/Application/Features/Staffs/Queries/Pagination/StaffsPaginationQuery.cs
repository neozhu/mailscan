// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;
using CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Queries.Pagination;

public class StaffsWithPaginationQuery : StaffAdvancedFilter, ICacheableRequest<PaginatedData<StaffDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => StaffCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => StaffCacheKey.MemoryCacheEntryOptions;
    public StaffAdvancedSpecification Specification => new StaffAdvancedSpecification(this);
}
    
public class StaffsWithPaginationQueryHandler :
         IRequestHandler<StaffsWithPaginationQuery, PaginatedData<StaffDto>>
{
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StaffsWithPaginationQueryHandler> _localizer;

        public StaffsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<StaffsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<StaffDto>> Handle(StaffsWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.Staffs.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                    .ProjectToPaginatedDataAsync<Staff, StaffDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
            return data;
        }
}