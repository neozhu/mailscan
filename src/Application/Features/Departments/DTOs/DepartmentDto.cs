// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.DTOs;

[Description("Departments")]
public class DepartmentDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = String.Empty;
    [Description("Address")]
    public string? Address { get; set; }
    [Description("Keywords")]
    public string? Keywords { get; set; }
    [Description("Description")]
    public string? Description { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
        }
    }
}

