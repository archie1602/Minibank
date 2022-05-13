using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.Currencies
{
    public class CurrencyRateResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, CurrencyResponseValueItem> Valute { get; set; }
    }
}