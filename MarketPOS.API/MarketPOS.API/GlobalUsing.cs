// GLOBAL USING IN LAYER API

global using System.Text.Json;

global using MarketPOS.API;
global using MarketPOS.Design;

global using MarketPOS.Shared;
global using MarketPOS.Shared.DTOs;
global using MarketPOS.Shared.DTOs.CategoryDto;
global using MarketPOS.Shared.DTOs.ProductDto;
global using MarketPOS.Shared.Eunms.ProductEunms;

global using MarketPOS.API.Middlewares;
global using MarketPOS.API.Middlewares.LocalizetionCustom;
global using MarketPOS.API.Middlewares.Filters;

global using MarketPOS.Infrastructure;
global using MarketPOS.Infrastructure.Exceptions;

global using MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;

global using MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
global using MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
global using MarketPOS.Application.Features.CQRS.CQRSCategory.Command;
global using MarketPOS.Application.Features.CQRS.CQRSProduct.Command;


global using MediatR;
global using FluentValidation;
global using FluentValidation.AspNetCore;

global using Microsoft.AspNetCore.Localization;
global using Microsoft.Extensions.Localization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;

global using NSwag;
global using NSwag.Generation.Processors;
global using NSwag.Generation.Processors.Contexts;
