﻿global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Postex.Payment.Infrastructure.EntityConfigurations;
global using Ardalis.Specification;
global using Postex.Payment.Infrastructure.Data;
global using Postex.SharedKernel.Interfaces;
global using Postex.SharedKernel.Paginations;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Postex.Payment.Infrastructure.Repositories;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Polly;
global using Polly.Contrib.WaitAndRetry;
global using Postex.Payment.Application.Contracts;
global using Postex.Payment.Infrastructure.PaymentMethods;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Routing;
global using System.Web;
global using Postex.Parcel.Domain.AggregatesModel.PaymentAggregate;
global using Postex.Payment.Infrastructure.PaymentMethods.Mellat;
