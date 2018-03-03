using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementParse
{
    class HistoryTransaction
    {
        public string account_number { get; set; }
        public string name { get; set; }
        public string tran_date { get; set; }
        public string tran_code { get; set; }
        public string account_id { get; set; }        
        public string description { get; set; }
        public double tran_amount { get; set; }
        public double balance { get; set; }
    }
}
