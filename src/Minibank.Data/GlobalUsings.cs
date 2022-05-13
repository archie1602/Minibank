global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http.Json;
global using System.Net.Http;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using System.ComponentModel.DataAnnotations.Schema;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Minibank.Core;
global using Minibank.Core.Exceptions;

global using Minibank.Core.Domains.Users;
global using Minibank.Core.Domains.BankAccounts;
global using Minibank.Core.Domains.TransferHistorys;
global using Minibank.Core.Domains.Currencies;

global using Minibank.Core.Domains.Users.Repositories;
global using Minibank.Core.Domains.BankAccounts.Repositories;
global using Minibank.Core.Domains.TransferHistorys.Repositories;

global using Minibank.Data.BankAccounts;
global using Minibank.Data.TransferHistorys;
global using Minibank.Data.Users;

global using Minibank.Data.BankAccounts.Repositories;
global using Minibank.Data.TransferHistorys.Repositories;
global using Minibank.Data.Users.Repositories;

global using Minibank.Data.Currencies;
global using Minibank.Data.Mappers;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using AutoMapper;