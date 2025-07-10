// global using directives for the application layer

// cqrs commands, queries, and handlers
global using MediatR;
global using AutoMapper;
global using FluentValidation;

global using Market.Domain.Entitys.DomainCategory;
global using Market.Domain.Entitys.DomainProduct;
global using Market.Domain.Entitys.Suppliers;

global using MarketPOS.Application.Services.Interfaces;
global using MarketPOS.Design.FactoryResult;
global using MarketPOS.Design.FactoryServices;
global using MarketPOS.Shared.DTOs;
global using MarketPOS.Shared.DTOs.CategoryDto;
global using MarketPOS.Shared.DTOs.ProductDto;
global using MarketPOS.Shared.Eunms.ProductEunms;

global using MarketPOS.Application.Common.Interfaces.ProductRepositorys;
global using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
global using MarketPOS.Application.Common.Interfaces.RepositorySupplier;
global using MarketPOS.Application.Common.Exceptions;
global using MarketPOS.Application.Common.Behaviors;
global using MarketPOS.Application.Common.HandlerBehaviors;
global using MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
global using MarketPOS.Application.Common.Helpers.IncludeHalpers;

global using Microsoft.Extensions.DependencyInjection;
global using Market.POS.Application.Services.Interfaces;
global using MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
global using MarketPOS.Application.Features.CQRS.CQRSProduct.Command;

//linq Youe Using and Framework
global using Microsoft.EntityFrameworkCore;
global using System.Linq.Expressions;

//logging
global using Microsoft.Extensions.Logging;

//localization
global using Microsoft.Extensions.Localization;
global using System.Reflection;
