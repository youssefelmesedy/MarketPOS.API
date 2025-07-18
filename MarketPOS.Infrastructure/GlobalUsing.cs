﻿// globale Using in layer Infrastructure

//framework using
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// entity using
global using Market.Domain.Entitys;
global using Market.Domain.Entitys.DomainCategory;
global using Market.Domain.Entitys.DomainProduct;
global using Market.Domain.Entitys.PurchaseInvoices;
global using Market.Domain.Entitys.Suppliers;

// global using 
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;

global using AutoMapper;
global using FluentValidation;

global using System.Linq.Dynamic.Core;
global using System.Linq.Expressions;

global using System.Text.Json;
global using System.Text.Json.Serialization;

global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;

global using MarketPOS.Infrastructure.Context;
global using MarketPOS.Application.Common.Interfaces;
global using MarketPOS.Application.Common.Exceptions;
global using MarketPOS.Shared.ExceptionDto;

global using Market.POS.Infrastructure.Repositories;
global using MarketPOS.Application.Common.Interfaces.ProductRepositorys;
global using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
global using MarketPOS.Application.Common.Interfaces.RepositorySupplier;

global using MarketPOS.Infrastructure.Services;
global using MarketPOS.Application.Services.Interfaces;
global using Market.POS.Application.Services.Interfaces;

global using Market.POS.Infrastructure.Services;
global using MarketPOS.Infrastructure.Repositories.ProductRepositorys;
global using MarketPOS.Infrastructure.Repositories.RepositoryCategory;
global using MarketPOS.Infrastructure.Repositories.RepositorySupplier;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;



