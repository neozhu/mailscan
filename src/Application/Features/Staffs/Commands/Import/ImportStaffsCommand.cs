// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;
using CleanArchitecture.Blazor.Application.Features.Staffs.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Commands.Import;

public class ImportStaffsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => StaffCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => StaffCacheKey.SharedExpiryTokenSource();
    public ImportStaffsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateStaffsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportStaffsCommandHandler :
             IRequestHandler<CreateStaffsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportStaffsCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportStaffsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly StaffDto _dto = new();

    public ImportStaffsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportStaffsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportStaffsCommand request, CancellationToken cancellationToken)
    {

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, StaffDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.LastName)], (row, item) => item.LastName = row[_localizer[_dto.GetMemberDescription(x=>x.LastName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.FirstName)], (row, item) => item.FirstName = row[_localizer[_dto.GetMemberDescription(x=>x.FirstName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.EmailAddress)], (row, item) => item.EmailAddress = row[_localizer[_dto.GetMemberDescription(x=>x.EmailAddress)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.PhoneNumber)], (row, item) => item.PhoneNumber = row[_localizer[_dto.GetMemberDescription(x=>x.PhoneNumber)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.Tag)], (row, item) => item.Tag = row[_localizer[_dto.GetMemberDescription(x=>x.Tag)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.DepartmentName)], (row, item) => item.DepartmentName = row[_localizer[_dto.GetMemberDescription(x=>x.DepartmentName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.DepartmentAddress)], (row, item) => item.DepartmentAddress = row[_localizer[_dto.GetMemberDescription(x=>x.DepartmentAddress)]].ToString() },
            }, _localizer[_dto.GetClassDescription()]).ConfigureAwait(true);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var dep = await _context.Departments.FirstOrDefaultAsync(x => x.Name == dto.DepartmentName, cancellationToken).ConfigureAwait(true);
                if(dep is null)
                {
                    dep = new Department() { Name = dto.DepartmentName,Address=dto.DepartmentAddress };
                }
                var exists = await _context.Staffs.AnyAsync(x => x.LastName == dto.LastName && x.FirstName == dto.FirstName, cancellationToken).ConfigureAwait(true);
                if (!exists)
                {
                    var item = _mapper.Map<Staff>(dto);
                    if (dep.Id > 0)
                    {
                        item.DepartmentId = dep.Id;
                    }
                    else
                    {
                        item.Department = dep;
                    }
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new StaffCreatedEvent(item));
                    _context.Staffs.Add(item);
                }
            }
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await Result<int>.SuccessAsync(result.Data.Count()).ConfigureAwait(false);
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }
    }
    public async Task<Result<byte[]>> Handle(CreateStaffsTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportStaffsCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                   _localizer[_dto.GetMemberDescription(x=>x.LastName)],
_localizer[_dto.GetMemberDescription(x=>x.FirstName)],
_localizer[_dto.GetMemberDescription(x=>x.EmailAddress)],
_localizer[_dto.GetMemberDescription(x=>x.PhoneNumber)],
_localizer[_dto.GetMemberDescription(x=>x.Tag)],
_localizer[_dto.GetMemberDescription(x=>x.DepartmentId)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

