// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.EventHandlers;

    public class ScanHistoryUpdatedEventHandler : INotificationHandler<ScanHistoryUpdatedEvent>
    {
        private readonly ILogger<ScanHistoryUpdatedEventHandler> _logger;

        public ScanHistoryUpdatedEventHandler(
            ILogger<ScanHistoryUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ScanHistoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
    }
