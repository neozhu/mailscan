// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.EventHandlers;

public class ScanHistoryCreatedEventHandler : INotificationHandler<ScanHistoryCreatedEvent>
{
        private readonly ILogger<ScanHistoryCreatedEventHandler> _logger;

        public ScanHistoryCreatedEventHandler(
            ILogger<ScanHistoryCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ScanHistoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
}
