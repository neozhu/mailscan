// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Features.ScanHistories.DTOs;

[Description("ScanHistories")]
public class ScanHistoryDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Recognizing Text")]
    public string? RecognizingText { get; set; }
    [Description("Match Status")]
    public string? MatchStatus { get; set; }
    [Description("Department")]
    public string? Department { get; set; }
    [Description("Fist Name")]
    public string? FistName { get; set; }
    [Description("Last Name")]
    public string? LastName { get; set; }
    [Description("Address")]
    public string? Address { get; set; }
    [Description("Elapsed Time")]
    public decimal? ElapsedTime { get; set; }
    [Description("Operator")]
    public string? Operator { get; set; }
    [Description("Scan Date Time")]
    public DateTime ScanDateTime { get; set; }
    [Description("Comments")]
    public string? Comments { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ScanHistory, ScanHistoryDto>().ReverseMap();
        }
    }
}

