// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.Commands.AddEdit;

public class AddEditScanHistoryCommandValidator : AbstractValidator<AddEditScanHistoryCommand>
{
    public AddEditScanHistoryCommandValidator()
    {
        RuleFor(v => v.MatchStatus)
         .MaximumLength(256)
         .NotEmpty();

    }

}

