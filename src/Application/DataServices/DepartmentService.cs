// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;

namespace CleanArchitecture.Blazor.Application.DataServices;
public class DepartmentService : IDepartmentService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFusionCache _cache;

    public DepartmentService(IServiceScopeFactory scopeFactory, IMapper mapper, IFusionCache cache)
    {
        var scope = scopeFactory.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        _mapper = mapper;
        _cache = cache;
    }
    public event Action? OnChange;
    public List<DepartmentDto>? DataSource { get; private set; } = new();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        DataSource = await _cache.GetOrSetAsync(DepartmentCacheKey.GetAllCacheKey, _ => _context.Departments.OrderBy(x => x.Name).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken), token: cancellationToken).ConfigureAwait(false);
    }

    public void Initialize()
    {
        DataSource = _cache.GetOrSet(DepartmentCacheKey.GetAllCacheKey, 
             _ => _context.Departments.OrderBy(x => x.Name)
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .ToList());
    }

    public async Task Refresh(CancellationToken cancellationToken = default)
    {
        _cache.Remove(DepartmentCacheKey.GetAllCacheKey, null, cancellationToken);
        DataSource = await _cache.GetOrSetAsync(DepartmentCacheKey.GetAllCacheKey, _ => _context.Departments.OrderBy(x => x.Name).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken), token: cancellationToken).ConfigureAwait(false);
    }
}
