// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;
namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Commands.AddEdit;

public class AddEditScanHistoryCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
          [Description("Recognizing Text")]
    public string? RecognizingText {get;set;} 
    [Description("Match Status")]
    public string? MatchStatus {get;set;} 
    [Description("Department")]
    public string? Department {get;set;} 
    [Description("Fist Name")]
    public string? FistName {get;set;} 
    [Description("Last Name")]
    public string? LastName {get;set;} 
    [Description("Address")]
    public string? Address {get;set;} 
    [Description("Elapsed Time")]
    public decimal? ElapsedTime {get;set;} 
    [Description("Operator")]
    public string? Operator {get;set;} 
    [Description("Scan Date Time")]
    public DateTime ScanDateTime {get;set;} 
    [Description("Comments")]
    public string? Comments {get;set;} 


      public string CacheKey => ScanHistoryCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => ScanHistoryCacheKey.SharedExpiryTokenSource();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ScanHistoryDto,AddEditScanHistoryCommand>(MemberList.None);
            CreateMap<AddEditScanHistoryCommand,ScanHistory>(MemberList.None);
         
        }
    }
}

    public class AddEditScanHistoryCommandHandler : IRequestHandler<AddEditScanHistoryCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditScanHistoryCommandHandler> _localizer;
        public AddEditScanHistoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditScanHistoryCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditScanHistoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = await _context.ScanHistories.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"ScanHistory with id: [{request.Id}] not found.");
                item = _mapper.Map(request, item);
				// raise a update domain event
				item.AddDomainEvent(new ScanHistoryUpdatedEvent(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = _mapper.Map<ScanHistory>(request);
                // raise a create domain event
				item.AddDomainEvent(new ScanHistoryCreatedEvent(item));
                _context.ScanHistories.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
           
        }
    }

