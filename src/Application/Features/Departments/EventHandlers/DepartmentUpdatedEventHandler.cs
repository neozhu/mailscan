// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

    public class DepartmentUpdatedEventHandler : INotificationHandler<DepartmentUpdatedEvent>
    {
        private readonly ILogger<DepartmentUpdatedEventHandler> _logger;

        public DepartmentUpdatedEventHandler(
            ILogger<DepartmentUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DepartmentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
    }
