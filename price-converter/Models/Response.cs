using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace price_converter.Models
{
    class Response
    {
        public string base_currency_id { get; set; }
        public string base_currency_name { get; set; }
        public DateTime base_price_last_updated { get; set; }
        public string quote_currency_id { get; set; }
        public string quote_currency_name { get; set; }
        public DateTime quote_price_last_updated { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
    }
}
