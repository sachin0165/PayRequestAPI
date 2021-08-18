using PayRequestWeb.Models;
using System.Threading.Tasks;

namespace PayRequestWeb.BlockchainApi
{
    public interface IBlockchainApi
    {
        Task<TransactionReceiptModel> GetReceipt(string txHash);

        Task<LocalCallResponse> GetUserBalance(string walletAddress);

        Task<TransactionResponseModel> CreateRequest(int id, string reason, string creatorWalletAddress, string recipientAddress, long amountInSatoshi, long expiry);

        Task<TransactionResponseModel> PayRequest(int id, string payerWalletAddress, long currentTime);

        Task<TransactionResponseModel> CancelRequest(int id, string payerWalletAddress, long currentTime);

    }
}
