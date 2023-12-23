// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class ScanHistory : BaseAuditableEntity
{
    public required string RecognizingText{ get; set; }
    public string? MatchStatus { get; set; }
    public string? Department { get; set; }
    public string? FistName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public decimal? ElapsedTime { get; set; }
    public string? Operator { get; set; }
    public DateTime ScanDateTime { get; set; } = DateTime.Now;
    public string? Comments { get; set; }


}
