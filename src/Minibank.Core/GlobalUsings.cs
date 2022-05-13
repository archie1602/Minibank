global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Net.Http;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Minibank.Core.Domains.Users;

global using Minibank.Core.Domains.Users.Services;
global using Minibank.Core.Domains.BankAccounts.Services;
global using Minibank.Core.Domains.TransferHistorys.Services;

global using Minibank.Core.Domains.Users.Repositories;
global using Minibank.Core.Domains.BankAccounts.Repositories;
global using Minibank.Core.Domains.TransferHistorys.Repositories;

global using Minibank.Core.Domains.BankAccounts;
global using Minibank.Core.Domains.Currencies;
global using Minibank.Core.Domains.TransferHistorys;

global using Minibank.Core.Exceptions;

// fluent validator namespaces:
global using FluentValidation;
global using FluentValidation.AspNetCore;