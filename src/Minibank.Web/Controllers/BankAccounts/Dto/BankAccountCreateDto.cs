using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountCreateDto
    {
        public int UserId { get; set; }
        public string CurrencyType { get; set; }
    }
}
