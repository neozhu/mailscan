// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;
using CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Queries.GetById;

public class GetStaffByIdQuery : ICacheableRequest<StaffDto>
{
   public required int Id { get; set; }
   public string CacheKey => StaffCacheKey.GetByIdCacheKey($"{Id}");
   public MemoryCacheEntryOptions? Options => StaffCacheKey.MemoryCacheEntryOptions;
}

public class GetStaffByIdQueryHandler :
     IRequestHandler<GetStaffByIdQuery, StaffDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetStaffByIdQueryHandler> _localizer;

    public GetStaffByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetStaffByIdQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<StaffDto> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Staffs.ApplySpecification(new StaffByIdSpecification(request.Id))
                     .ProjectTo<StaffDto>(_mapper.ConfigurationProvider)
                     .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Staff with id: [{request.Id}] not found.");
        return data;
    }
}
