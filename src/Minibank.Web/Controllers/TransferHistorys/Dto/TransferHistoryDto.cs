using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.TransferHistorys.Dto
{
    public class TransferHistoryDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
    }
}
