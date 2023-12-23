// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Queries.GetAll;

public class GetAllStaffsQuery : ICacheableRequest<IEnumerable<StaffDto>>
{
   public string CacheKey => StaffCacheKey.GetAllCacheKey;
   public MemoryCacheEntryOptions? Options => StaffCacheKey.MemoryCacheEntryOptions;
}

public class GetAllStaffsQueryHandler :
     IRequestHandler<GetAllStaffsQuery, IEnumerable<StaffDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllStaffsQueryHandler> _localizer;

    public GetAllStaffsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllStaffsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<StaffDto>> Handle(GetAllStaffsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Staffs
                     .ProjectTo<StaffDto>(_mapper.ConfigurationProvider)
                     .AsNoTracking()
                     .ToListAsync(cancellationToken);
        return data;
    }
}


