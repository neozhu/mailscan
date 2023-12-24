// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.DataServices;

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

public class DepartmentUpdatedEventHandler : INotificationHandler<DepartmentUpdatedEvent>
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentUpdatedEventHandler> _logger;

    public DepartmentUpdatedEventHandler(
        IDepartmentService departmentService,
        ILogger<DepartmentUpdatedEventHandler> logger
        )
    {
        _departmentService = departmentService;
        _logger = logger;
    }
    public async Task Handle(DepartmentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _departmentService.Refresh(cancellationToken).ConfigureAwait(false);
    }
}
