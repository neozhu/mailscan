// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.Caching;


namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Commands.Delete;

    public class DeleteScanHistoryCommand:  ICacheInvalidatorRequest<Result<int>>
    {
      public int[] Id {  get; }
      public string CacheKey => ScanHistoryCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => ScanHistoryCacheKey.SharedExpiryTokenSource();
      public DeleteScanHistoryCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteScanHistoryCommandHandler : 
                 IRequestHandler<DeleteScanHistoryCommand, Result<int>>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteScanHistoryCommandHandler> _localizer;
        public DeleteScanHistoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteScanHistoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(DeleteScanHistoryCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.ScanHistories.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
			    // raise a delete domain event
				item.AddDomainEvent(new ScanHistoryDeletedEvent(item));
                _context.ScanHistories.Remove(item);
            }
            var result = await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result);
        }

    }

