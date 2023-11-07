using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class CreateCashoutResponse
    {
        public Guid BatchId { get; set; }
        public List<CreateCashoutItemResponce> CashOutItems { get; set; }

    }
    public class CreateCashoutItemResponce
    {
        public Guid TransferId { get; set; }
        public DateTime CreateDate { get; set; }
        public string status { get; set; }
    }
}
