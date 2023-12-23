// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Queries.GetAll;

public class GetAllScanHistoriesQuery : ICacheableRequest<IEnumerable<ScanHistoryDto>>
{
   public string CacheKey => ScanHistoryCacheKey.GetAllCacheKey;
   public MemoryCacheEntryOptions? Options => ScanHistoryCacheKey.MemoryCacheEntryOptions;
}

public class GetAllScanHistoriesQueryHandler :
     IRequestHandler<GetAllScanHistoriesQuery, IEnumerable<ScanHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllScanHistoriesQueryHandler> _localizer;

    public GetAllScanHistoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllScanHistoriesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<ScanHistoryDto>> Handle(GetAllScanHistoriesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.ScanHistories
                     .ProjectTo<ScanHistoryDto>(_mapper.ConfigurationProvider)
                     .AsNoTracking()
                     .ToListAsync(cancellationToken);
        return data;
    }
}


