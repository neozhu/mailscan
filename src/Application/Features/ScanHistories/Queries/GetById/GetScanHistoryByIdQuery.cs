// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Queries.GetById;

public class GetScanHistoryByIdQuery : ICacheableRequest<ScanHistoryDto>
{
   public required int Id { get; set; }
   public string CacheKey => ScanHistoryCacheKey.GetByIdCacheKey($"{Id}");
   public MemoryCacheEntryOptions? Options => ScanHistoryCacheKey.MemoryCacheEntryOptions;
}

public class GetScanHistoryByIdQueryHandler :
     IRequestHandler<GetScanHistoryByIdQuery, ScanHistoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetScanHistoryByIdQueryHandler> _localizer;

    public GetScanHistoryByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetScanHistoryByIdQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<ScanHistoryDto> Handle(GetScanHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.ScanHistories.ApplySpecification(new ScanHistoryByIdSpecification(request.Id))
                     .ProjectTo<ScanHistoryDto>(_mapper.ConfigurationProvider)
                     .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ScanHistory with id: [{request.Id}] not found.");
        return data;
    }
}
