﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.Import;

public class ImportDepartmentsCommandValidator : AbstractValidator<ImportDepartmentsCommand>
{
        public ImportDepartmentsCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

