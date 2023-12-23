// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.EventHandlers;

    public class ScanHistoryDeletedEventHandler : INotificationHandler<ScanHistoryDeletedEvent>
    {
        private readonly ILogger<ScanHistoryDeletedEventHandler> _logger;

        public ScanHistoryDeletedEventHandler(
            ILogger<ScanHistoryDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ScanHistoryDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
    }
