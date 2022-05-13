global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;

global using Moq;
global using Xunit;

global using FluentValidation;

global using Minibank.Core.Domains.BankAccounts;
global using Minibank.Core.Domains.BankAccounts.Repositories;
global using Minibank.Core.Domains.Users;
global using Minibank.Core.Domains.Users.Repositories;
global using Minibank.Core.Domains.Users.Validators;
global using Minibank.Core.Domains.BankAccounts.Validators;
global using Minibank.Core.Exceptions;
global using Minibank.Core.Domains.Users.Services;
global using Minibank.Core.Domains.BankAccounts.Services;
global using Minibank.Core.Tests.Data.Users;
global using Minibank.Core.Domains.TransferHistorys.Repositories;
global using Minibank.Core.Domains.Currencies;
global using Minibank.Core.Tests.Data.BankAccounts;
global using Minibank.Core.Domains.TransferHistorys;