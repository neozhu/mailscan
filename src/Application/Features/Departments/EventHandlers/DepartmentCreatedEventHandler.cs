// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

public class DepartmentCreatedEventHandler : INotificationHandler<DepartmentCreatedEvent>
{
        private readonly ILogger<DepartmentCreatedEventHandler> _logger;

        public DepartmentCreatedEventHandler(
            ILogger<DepartmentCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DepartmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
}
