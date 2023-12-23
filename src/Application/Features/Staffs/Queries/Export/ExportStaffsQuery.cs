// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Specifications;
using CleanArchitecture.Blazor.Application.Features.Staffs.Queries.Pagination;

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Queries.Export;

public class ExportStaffsQuery : StaffAdvancedFilter, IRequest<Result<byte[]>>
{
      public StaffAdvancedSpecification Specification => new StaffAdvancedSpecification(this);
}
    
public class ExportStaffsQueryHandler :
         IRequestHandler<ExportStaffsQuery, Result<byte[]>>
{
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportStaffsQueryHandler> _localizer;
        private readonly StaffDto _dto = new();
        public ExportStaffsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportStaffsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }
        #nullable disable warnings
        public async Task<Result<byte[]>> Handle(ExportStaffsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Staffs.ApplySpecification(request.Specification)
                       .OrderBy($"{request.OrderBy} {request.SortDirection}")
                       .ProjectTo<StaffDto>(_mapper.ConfigurationProvider)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<StaffDto, object?>>()
                {
                    // TODO: Define the fields that should be exported, for example:
                    {_localizer[_dto.GetMemberDescription(x=>x.LastName)],item => item.LastName}, 
{_localizer[_dto.GetMemberDescription(x=>x.FirstName)],item => item.FirstName}, 
{_localizer[_dto.GetMemberDescription(x=>x.EmailAddress)],item => item.EmailAddress}, 
{_localizer[_dto.GetMemberDescription(x=>x.PhoneNumber)],item => item.PhoneNumber}, 
{_localizer[_dto.GetMemberDescription(x=>x.Tag)],item => item.Tag}, 
{_localizer[_dto.GetMemberDescription(x=>x.DepartmentId)],item => item.DepartmentId}, 

                }
                , _localizer[_dto.GetClassDescription()]);
            return await Result<byte[]>.SuccessAsync(result);
        }
}
