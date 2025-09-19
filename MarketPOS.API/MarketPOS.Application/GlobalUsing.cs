// global using directives for the application layer

// cqrs commands, queries, and handlers
global using AutoMapper;
global using FluentValidation;
global using Market.Domain.Entitys.DomainCategory;
global using Market.Domain.Entitys.DomainProduct;
global using Market.Domain.Entitys.Suppliers;
global using MarketPOS.Application.Common.Behaviors;
global using MarketPOS.Application.Common.Exceptions;
global using MarketPOS.Application.Common.HandlerBehaviors;
global using MarketPOS.Application.Common.Helpers.IncludeHalpers;
global using MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
global using MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
global using MarketPOS.Application.Features.CQRS.CQRSProduct.Command;
global using MarketPOS.Design.FactoryResult;
global using MarketPOS.Design.FactoryServices;
global using MarketPOS.Shared.DTOs;
global using MarketPOS.Shared.DTOs.CategoryDto;
global using MarketPOS.Shared.DTOs.ProductDto;
global using MarketPOS.Shared.Eunms.ProductEunms;
global using MediatR;
//linq Youe Using and Framework
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
//localization
global using Microsoft.Extensions.Localization;
//logging
global using Microsoft.Extensions.Logging;
global using System.Linq.Expressions;
global using System.Reflection;
