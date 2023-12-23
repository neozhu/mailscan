// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Queries.Pagination;

public class ScanHistoriesWithPaginationQuery : ScanHistoryAdvancedFilter, ICacheableRequest<PaginatedData<ScanHistoryDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ScanHistoryCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => ScanHistoryCacheKey.MemoryCacheEntryOptions;
    public ScanHistoryAdvancedSpecification Specification => new ScanHistoryAdvancedSpecification(this);
}
    
public class ScanHistoriesWithPaginationQueryHandler :
         IRequestHandler<ScanHistoriesWithPaginationQuery, PaginatedData<ScanHistoryDto>>
{
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ScanHistoriesWithPaginationQueryHandler> _localizer;

        public ScanHistoriesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ScanHistoriesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ScanHistoryDto>> Handle(ScanHistoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.ScanHistories.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                    .ProjectToPaginatedDataAsync<ScanHistory, ScanHistoryDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
            return data;
        }
}