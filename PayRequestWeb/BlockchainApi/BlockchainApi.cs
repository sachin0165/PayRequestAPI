using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayRequestWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PayRequestWeb.BlockchainApi
{
    public class BlockchainApi : IBlockchainApi
    {
        private readonly IOptions<Configuration> _appSettings;

        public BlockchainApi(IOptions<Configuration> appSettings)
        {
            _appSettings = appSettings;
        }

        #region Private

        private static StringContent PrepareJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private ContractCallModel PrepareContractCallRequest()
        {
            return new ContractCallModel
            {
                Amount = 0,
                ContractAddress = _appSettings.Value.ContractAddress,
                WalletName = _appSettings.Value.WalletName,
                Password = _appSettings.Value.WalletPassword,
                AccountName = _appSettings.Value.AccountName,
                Outpoints = null,
                FeeAmount = _appSettings.Value.FeeAmount,
                GasPrice = Convert.ToInt32(_appSettings.Value.GasPrice),
                GasLimit = Convert.ToInt32(_appSettings.Value.GasLimit)
            };
        }

        private LocalCallModel PrepareLocalCallRequest(string methodName, string senderWalletAddress)
        {
            return new LocalCallModel
            {
                ContractAddress = _appSettings.Value.ContractAddress,
                MethodName = methodName,
                Amount = 0,
                GasPrice = Convert.ToInt32(_appSettings.Value.GasPrice),
                GasLimit = Convert.ToInt32(_appSettings.Value.GasLimit),
                Sender = senderWalletAddress
            };
        }

        #endregion

        #region Methods

        public async Task<TransactionReceiptModel> GetReceipt(string txHash)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    var response = await client.GetAsync($"SmartContracts/receipt?txHash={txHash}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<TransactionReceiptModel>(content);
                        return result;
                    }
                    return new TransactionReceiptModel { Success = false, Error = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionReceiptModel { Success = false, Error = ex.Message };
            }
        }

        public async Task<LocalCallResponse> GetUserBalance(string walletAddress)
        {

            var requestBody = PrepareLocalCallRequest("GetBalance", walletAddress);

            var parameters = new List<string> { $"{9}#{walletAddress}" };

            requestBody.Parameters = parameters;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("SmartContracts/local-call", PrepareJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LocalCallResponse>(content);
                }

                return new LocalCallResponse { ErrorMessage = response.RequestMessage.ToString() };
            }
        }

        public async Task<TransactionResponseModel> CreateRequest(int id, string reason, string creatorWalletAddress, string recipientAddress, long amountInSatoshi, long expiry)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "CreateRequest";
                requestBody.Sender = creatorWalletAddress;

                var parameters = new List<string>
                {
                    $"{5}#{id}",
                    $"{4}#{reason}",
                    $"{9}#{recipientAddress}",
                    $"{7}#{amountInSatoshi}",
                    $"{7}#{expiry}"
                };

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", PrepareJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponseModel>(content);
                    }

                    return new TransactionResponseModel { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponseModel { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponseModel> PayRequest(int id, string payerWalletAddress, long currentTime)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "PayRequest";
                requestBody.Sender = payerWalletAddress;

                var parameters = new List<string> { $"{5}#{id}", $"{7}#{currentTime}" };

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", PrepareJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponseModel>(content);
                    }

                    return new TransactionResponseModel { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponseModel { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponseModel> CancelRequest(int id, string payerWalletAddress, long currentTime)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "CancelRequest";
                requestBody.Sender = payerWalletAddress;

                var parameters = new List<string> { $"{5}#{id}" };

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", PrepareJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponseModel>(content);
                    }

                    return new TransactionResponseModel { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponseModel { Success = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
