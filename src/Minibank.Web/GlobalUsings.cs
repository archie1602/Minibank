global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http.Json;
global using System.Net.Http;
global using System.Text;
global using System.Text.Json;
global using System.IO;
global using System.Reflection;
global using System.Threading;
global using System.Threading.Tasks;

global using Minibank.Core;
global using Minibank.Data;
global using Minibank.Web.Middlewares;

global using Microsoft.EntityFrameworkCore;

global using Minibank.Core.Exceptions;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.OpenApi.Models;
global using Microsoft.OpenApi.Extensions;
global using Microsoft.IdentityModel.Tokens;

global using Minibank.Core.Domains.Users.Services;
global using Minibank.Core.Domains.BankAccounts.Services;
global using Minibank.Core.Domains.TransferHistorys.Services;

global using Minibank.Core.Domains.Users;
global using Minibank.Core.Domains.BankAccounts;
global using Minibank.Core.Domains.TransferHistorys;

global using Minibank.Web.Controllers.Users.Dto;
global using Minibank.Web.Controllers.BankAccounts.Dto;
global using Minibank.Web.Controllers.TransferHistorys.Dto;

global using Microsoft.Extensions.Hosting;

global using Minibank.Web.HostedServices;

global using Minibank.Web.Mappers;
global using Minibank.Data.Mappers;

global using AutoMapper;