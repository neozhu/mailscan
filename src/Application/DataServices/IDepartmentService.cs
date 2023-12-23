// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;

namespace CleanArchitecture.Blazor.Application.DataServices;
public interface IDepartmentService
{
    List<DepartmentDto>? DataSource { get; }

    event Action? OnChange;

    void Initialize();
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task Refresh(CancellationToken cancellationToken = default);
}
