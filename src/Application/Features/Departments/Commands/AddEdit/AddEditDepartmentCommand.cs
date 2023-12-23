// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.AddEdit;

public class AddEditDepartmentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = String.Empty;
    [Description("Address")]
    public string? Address { get; set; }
    [Description("Keywords")]
    public string? Keywords { get; set; }
    [Description("Description")]
    public string? Description { get; set; }


    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DepartmentDto, AddEditDepartmentCommand>(MemberList.None);
            CreateMap<AddEditDepartmentCommand, Department>(MemberList.None);

        }
    }
}

public class AddEditDepartmentCommandHandler : IRequestHandler<AddEditDepartmentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditDepartmentCommandHandler> _localizer;
    public AddEditDepartmentCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditDepartmentCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(true) ?? throw new NotFoundException($"Department with id: [{request.Id}] not found.");
            item = _mapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new DepartmentUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await Result<int>.SuccessAsync(item.Id).ConfigureAwait(false);
        }
        else
        {
            var item = _mapper.Map<Department>(request);
            // raise a create domain event
            item.AddDomainEvent(new DepartmentCreatedEvent(item));
            _context.Departments.Add(item);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await Result<int>.SuccessAsync(item.Id).ConfigureAwait(false);
        }

    }
}

