// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.DataServices;

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

public class DepartmentCreatedEventHandler : INotificationHandler<DepartmentCreatedEvent>
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentCreatedEventHandler> _logger;

    public DepartmentCreatedEventHandler(
        IDepartmentService departmentService,
        ILogger<DepartmentCreatedEventHandler> logger
        )
    {
        _departmentService = departmentService;
        _logger = logger;
    }
    public async Task Handle(DepartmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _departmentService.Refresh(cancellationToken).ConfigureAwait(false);
    }
}
