// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Staffs.Commands.Delete;

    public class DeleteStaffCommand:  ICacheInvalidatorRequest<Result<int>>
    {
      public int[] Id {  get; }
      public string CacheKey => StaffCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => StaffCacheKey.SharedExpiryTokenSource();
      public DeleteStaffCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteStaffCommandHandler : 
                 IRequestHandler<DeleteStaffCommand, Result<int>>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteStaffCommandHandler> _localizer;
        public DeleteStaffCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteStaffCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Staffs.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
			    // raise a delete domain event
				item.AddDomainEvent(new StaffDeletedEvent(item));
                _context.Staffs.Remove(item);
            }
            var result = await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result);
        }

    }

