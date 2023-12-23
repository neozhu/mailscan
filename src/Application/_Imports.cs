global using System.ComponentModel;
global using System.Data;
global using System.Globalization;
global using System.Linq.Dynamic.Core;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Text.Json;
global using Ardalis.Specification;
global using AutoMapper;
global using AutoMapper.QueryableExtensions;
global using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
global using CleanArchitecture.Blazor.Application.Common.Extensions;
global using CleanArchitecture.Blazor.Application.Common.Interfaces;
global using CleanArchitecture.Blazor.Application.Common.Interfaces.Caching;
global using CleanArchitecture.Blazor.Application.Common.Models;
global using CleanArchitecture.Blazor.Domain.Common.Enums;
global using CleanArchitecture.Blazor.Domain.Common.Events;
global using CleanArchitecture.Blazor.Domain.Entities;
global using CleanArchitecture.Blazor.Domain.Events;
global using FluentValidation;
global using LazyCache;
global using MediatR;
global using MediatR.Pipeline;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;
