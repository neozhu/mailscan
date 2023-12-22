// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Staff : BaseAuditableEntity
{
    public string LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Tag { get; set; }
    public int? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }


}
