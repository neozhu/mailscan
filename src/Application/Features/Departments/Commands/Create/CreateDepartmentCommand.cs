// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel;
using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.Create;

public class CreateDepartmentCommand: ICacheInvalidatorRequest<Result<int>>
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
             CreateMap<DepartmentDto,CreateDepartmentCommand>(MemberList.None);
             CreateMap<CreateDepartmentCommand,Department>(MemberList.None);
        }
    }
}
    
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateDepartmentCommand> _localizer;
        public CreateDepartmentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateDepartmentCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
           var item = _mapper.Map<Department>(request);
           // raise a create domain event
	       item.AddDomainEvent(new DepartmentCreatedEvent(item));
           _context.Departments.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

