// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Staffs.EventHandlers;

public class StaffCreatedEventHandler : INotificationHandler<StaffCreatedEvent>
{
        private readonly ILogger<StaffCreatedEventHandler> _logger;

        public StaffCreatedEventHandler(
            ILogger<StaffCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(StaffCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
}
