﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.AddEdit;

public class AddEditDepartmentCommandValidator : AbstractValidator<AddEditDepartmentCommand>
{
    public AddEditDepartmentCommandValidator()
    {
            RuleFor(v => v.Name)
                .MaximumLength(256)
                .NotEmpty();
       
     }

}
