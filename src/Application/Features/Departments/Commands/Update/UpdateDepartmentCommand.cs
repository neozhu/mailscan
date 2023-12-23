// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel;
using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.Update;

public class UpdateDepartmentCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
            [Description("Name")]
    public string Name {get;set;} = String.Empty; 
    [Description("Address")]
    public string? Address {get;set;} 
    [Description("Keywords")]
    public string? Keywords {get;set;} 
    [Description("Description")]
    public string? Description {get;set;} 

        public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource();
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DepartmentDto,UpdateDepartmentCommand>(MemberList.None);
            CreateMap<UpdateDepartmentCommand,Department>(MemberList.None);
        }
    }
}

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateDepartmentCommandHandler> _localizer;
        public UpdateDepartmentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateDepartmentCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {

           var item =await _context.Departments.FindAsync( new object[] { request.Id }, cancellationToken)?? throw new NotFoundException($"Department with id: [{request.Id}] not found.");
           item = _mapper.Map(request, item);
		    // raise a update domain event
		   item.AddDomainEvent(new DepartmentUpdatedEvent(item));
           await _context.SaveChangesAsync(cancellationToken);
           return await Result<int>.SuccessAsync(item.Id);
        }
    }

