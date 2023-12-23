// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Staffs.Commands.AddEdit;

public class AddEditStaffCommandValidator : AbstractValidator<AddEditStaffCommand>
{
    public AddEditStaffCommandValidator()
    {
        RuleFor(v => v.LastName)
            .MaximumLength(256)
            .NotEmpty();
        RuleFor(v => v.DepartmentId).NotNull();

    }

}

