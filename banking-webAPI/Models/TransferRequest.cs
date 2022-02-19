using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace banking_webAPI.Models
{
    public class TransferRequest
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public int Amount { get; set; }
    }
}
