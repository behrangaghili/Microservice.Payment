using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postex.Payment.Domain;


namespace Postex.Payment.Application.Contracts
{
    public interface IWalletServiceClient
    {
        Task<WalletResponse> DepositAsync(WalletRequest walletRequest, Guid correlationId);
        Task<WalletResponse> WithdrawAsync(WalletRequest walletRequest, Guid correlationId);
    }

    


}
