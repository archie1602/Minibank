using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountTransferFeesInfoDto
    {
        public string CurrencyType { get; set; }
        public decimal Commission { get; set; }
    }
}
