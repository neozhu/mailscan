// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Staffs.DTOs;

[Description("Staffs")]
public class StaffDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Last Name")]
    public string? LastName { get; set; }
    [Description("First Name")]
    public string? FirstName { get; set; }
    [Description("Email Address")]
    public string? EmailAddress { get; set; }
    [Description("Phone Number")]
    public string? PhoneNumber { get; set; }
    [Description("Tag")]
    public string? Tag { get; set; }
    [Description("Department Id")]
    public int? DepartmentId { get; set; }
    [Description("Department Name")]
    public string? DepartmentName { get; set; }
    [Description("Department Address")]
    public string? DepartmentAddress { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Staff, StaffDto>().ForMember(x=>x.DepartmentName,y=>y.MapFrom(x=>$"{x.Department.Name}"))
                .ForMember(x => x.DepartmentAddress, y => y.MapFrom(x => $"{x.Department.Address}"));

            CreateMap<StaffDto, Staff>(MemberList.None);
        }
    }
}

