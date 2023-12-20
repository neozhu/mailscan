﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
        public CreateCustomerCommandValidator()
        {
           
            RuleFor(v => v.Name)
                 .MaximumLength(256)
                 .NotEmpty();
        
        }
       
}

