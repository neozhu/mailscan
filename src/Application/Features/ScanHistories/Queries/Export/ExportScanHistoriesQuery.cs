// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Specifications;
using CleanArchitecture.Blazor.Application.Features.ScanHistories.Queries.Pagination;

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Queries.Export;

public class ExportScanHistoriesQuery : ScanHistoryAdvancedFilter, IRequest<Result<byte[]>>
{
      public ScanHistoryAdvancedSpecification Specification => new ScanHistoryAdvancedSpecification(this);
}
    
public class ExportScanHistoriesQueryHandler :
         IRequestHandler<ExportScanHistoriesQuery, Result<byte[]>>
{
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportScanHistoriesQueryHandler> _localizer;
        private readonly ScanHistoryDto _dto = new();
        public ExportScanHistoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportScanHistoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }
        #nullable disable warnings
        public async Task<Result<byte[]>> Handle(ExportScanHistoriesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.ScanHistories.ApplySpecification(request.Specification)
                       .OrderBy($"{request.OrderBy} {request.SortDirection}")
                       .ProjectTo<ScanHistoryDto>(_mapper.ConfigurationProvider)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ScanHistoryDto, object?>>()
                {
                    // TODO: Define the fields that should be exported, for example:
                    {_localizer[_dto.GetMemberDescription(x=>x.RecognizingText)],item => item.RecognizingText}, 
{_localizer[_dto.GetMemberDescription(x=>x.MatchStatus)],item => item.MatchStatus}, 
{_localizer[_dto.GetMemberDescription(x=>x.Department)],item => item.Department}, 
{_localizer[_dto.GetMemberDescription(x=>x.FistName)],item => item.FistName}, 
{_localizer[_dto.GetMemberDescription(x=>x.LastName)],item => item.LastName}, 
{_localizer[_dto.GetMemberDescription(x=>x.Address)],item => item.Address}, 
{_localizer[_dto.GetMemberDescription(x=>x.ElapsedTime)],item => item.ElapsedTime}, 
{_localizer[_dto.GetMemberDescription(x=>x.Operator)],item => item.Operator}, 
{_localizer[_dto.GetMemberDescription(x=>x.ScanDateTime)],item => item.ScanDateTime}, 
{_localizer[_dto.GetMemberDescription(x=>x.Comments)],item => item.Comments}, 

                }
                , _localizer[_dto.GetClassDescription()]);
            return await Result<byte[]>.SuccessAsync(result);
        }
}
