// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Staffs.EventHandlers;

public class StaffUpdatedEventHandler : INotificationHandler<StaffUpdatedEvent>
{
    private readonly ILogger<StaffUpdatedEventHandler> _logger;

    public StaffUpdatedEventHandler(
        ILogger<StaffUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(StaffUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
