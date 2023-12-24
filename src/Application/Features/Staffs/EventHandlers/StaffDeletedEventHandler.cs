// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Staffs.EventHandlers;

public class StaffDeletedEventHandler : INotificationHandler<StaffDeletedEvent>
{
    private readonly ILogger<StaffDeletedEventHandler> _logger;

    public StaffDeletedEventHandler(
        ILogger<StaffDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(StaffDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
