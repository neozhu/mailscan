// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
using CleanArchitecture.Blazor.Application.Features.Departments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.GetById;

public class GetDepartmentByIdQuery : ICacheableRequest<DepartmentDto>
{
   public required int Id { get; set; }
   public string CacheKey => DepartmentCacheKey.GetByIdCacheKey($"{Id}");
   public MemoryCacheEntryOptions? Options => DepartmentCacheKey.MemoryCacheEntryOptions;
}

public class GetDepartmentByIdQueryHandler :
     IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetDepartmentByIdQueryHandler> _localizer;

    public GetDepartmentByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetDepartmentByIdQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Departments.ApplySpecification(new DepartmentByIdSpecification(request.Id))
                     .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                     .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Department with id: [{request.Id}] not found.");
        return data;
    }
}
