using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Parcel.Domain.AggregatesModel.Wallet
{
    public enum TransactionTypes : int
    {
        OnlineCharge = 1,
        Refund = 3
    }
}
