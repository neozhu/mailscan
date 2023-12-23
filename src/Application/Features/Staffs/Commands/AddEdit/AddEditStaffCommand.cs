// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Staffs.Commands.AddEdit;

public class AddEditStaffCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Last Name")]
    public string? LastName { get; set; }
    [Description("First Name")]
    public string? FirstName { get; set; }
    [Description("Email Address")]
    public string? EmailAddress { get; set; }
    [Description("Phone Number")]
    public string? PhoneNumber { get; set; }
    [Description("Tag")]
    public string? Tag { get; set; }
    [Description("Department Id")]
    public int? DepartmentId { get; set; }


    public string CacheKey => StaffCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => StaffCacheKey.SharedExpiryTokenSource();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<StaffDto, AddEditStaffCommand>(MemberList.None);
            CreateMap<AddEditStaffCommand, Staff>(MemberList.None);

        }
    }
}

public class AddEditStaffCommandHandler : IRequestHandler<AddEditStaffCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditStaffCommandHandler> _localizer;
    public AddEditStaffCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditStaffCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditStaffCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Staffs.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException($"Staff with id: [{request.Id}] not found.");
            item = _mapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new StaffUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await Result<int>.SuccessAsync(item.Id).ConfigureAwait(false);
        }
        else
        {
            var item = _mapper.Map<Staff>(request);
            // raise a create domain event
            item.AddDomainEvent(new StaffCreatedEvent(item));
            _context.Staffs.Add(item);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await Result<int>.SuccessAsync(item.Id).ConfigureAwait(false);
        }

    }
}

