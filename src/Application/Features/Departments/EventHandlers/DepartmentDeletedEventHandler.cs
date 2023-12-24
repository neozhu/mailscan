// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.DataServices;

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

public class DepartmentDeletedEventHandler : INotificationHandler<DepartmentDeletedEvent>
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentDeletedEventHandler> _logger;

    public DepartmentDeletedEventHandler(
        IDepartmentService departmentService,
        ILogger<DepartmentDeletedEventHandler> logger
        )
    {
        _departmentService = departmentService;
        _logger = logger;
    }
    public async Task Handle(DepartmentDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _departmentService.Refresh(cancellationToken).ConfigureAwait(false);
    }
}
