// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Department : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }

    public ICollection<Staff>? Staffs { get; set; }
}
