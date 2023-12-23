// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Commands.Create;

public class CreateScanHistoryCommand: ICacheInvalidatorRequest<Result<int>>
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
             CreateMap<ScanHistoryDto,CreateScanHistoryCommand>(MemberList.None);
             CreateMap<CreateScanHistoryCommand,ScanHistory>(MemberList.None);
        }
    }
}
    
    public class CreateScanHistoryCommandHandler : IRequestHandler<CreateScanHistoryCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateScanHistoryCommand> _localizer;
        public CreateScanHistoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateScanHistoryCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateScanHistoryCommand request, CancellationToken cancellationToken)
        {
           var item = _mapper.Map<ScanHistory>(request);
           // raise a create domain event
	       item.AddDomainEvent(new ScanHistoryCreatedEvent(item));
           _context.ScanHistories.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

