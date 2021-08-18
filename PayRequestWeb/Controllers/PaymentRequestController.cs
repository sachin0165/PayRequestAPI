using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayRequestWeb.BlockchainApi;
using PayRequestWeb.DataService;
using PayRequestWeb.Helpers;
using PayRequestWeb.Models;
using System;
using System.Threading.Tasks;

namespace PayRequestWeb.Controllers
{
    [Authorize]
    [Route("api/payment-request")]
    public class PaymentRequestController : BaseController
    {
        private readonly IPaymentRequestService _paymentRequestService;
        private readonly IBlockchainApi _blockchainApi;
        private readonly IOptions<Configuration> _configuration;

        public PaymentRequestController(IPaymentRequestService paymentRequestService,
            IBlockchainApi blockchainApi,
            IOptions<Configuration> configuration)
        {
            _paymentRequestService = paymentRequestService;
            _blockchainApi = blockchainApi;
            _configuration = configuration;
        }

        //TODO: add model validation
        [HttpPost("create")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequest request)
        {
            request.RequestGuid = Guid.NewGuid();
            request.PaymentStatus = PaymentStatus.AwaitingPayment;
            request.ExpiryInUnixTimestamp = request.Expiry.ConvertToUnixTimestamp();
            request.AmountInSatoshi = Convert.ToInt64(request.Amount * ApiConstants.OneSatoshi);

            var requestId = await _paymentRequestService.CreateRequest(request);
            if (requestId == 0)
                return BadRequest("Something went wrong");

            var createRequest = await _blockchainApi.CreateRequest(requestId, request.Reason, request.FromAddress, request.ToAddress, request.AmountInSatoshi, request.ExpiryInUnixTimestamp);
            if (!createRequest.Success)
            {
                return BadRequest("An error occurred while creating request into Blockchain!");
            }

            await Task.Delay(_configuration.Value.BlockTime);

            var receipt = await _blockchainApi.GetReceipt(createRequest.TransactionId);
            if (!receipt.Success)
                return BadRequest("An error occurred while fetching receipt!");

            return Success(new { }, "Request created successfully.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRequest()
        {
            var requests = await _paymentRequestService.GetAllRequests(User.GetUserWalletAddress());
            if (requests == null)
                return BadRequest("No record found");

            return Success(new
            {
                requests = requests
            });
        }

        [HttpGet("get-balance")]
        public async Task<IActionResult> GetBalance()
        {
            var balanceResult = await _blockchainApi.GetUserBalance(User.GetUserWalletAddress());
            if (balanceResult.ErrorMessage != null)
                return BadRequest(balanceResult.ErrorMessage);

            return Success(new BalanceResponse
            {
                Balance = Decimal.Divide(balanceResult.Return, ApiConstants.OneSatoshi)
            });            
        }

        [HttpGet("{requestGuid}")]
        public async Task<IActionResult> GetRequestById(Guid requestGuid)
        {
            var request = await _paymentRequestService.GetRequestByGuid(requestGuid);
            if (request == null)
                return BadRequest("Request with provided id is not found.");

            return Success(request);
        }

        [HttpPut("{requestGuid}/cancel")]
        public async Task<IActionResult> CancelRequest(Guid requestGuid)
        {
            var request = await _paymentRequestService.GetRequestByGuid(requestGuid);
            if (request == null)
                return BadRequest("Request with provided id is not found.");

            var changePaymentRequest = new ChangePaymentRequestStatus()
            {
                Id = request.Id,
                PaymentStatus = PaymentStatus.Canceled,
                CancellationTimeUtc = DateTime.UtcNow
            };

            var isUpdated = await _paymentRequestService.ChangeStatus(changePaymentRequest);
            if (isUpdated == 0)
                return BadRequest("Something went wrong");

            var cancelRequest = await _blockchainApi.CancelRequest(request.Id, User.GetUserWalletAddress(), DateTime.UtcNow.ConvertToUnixTimestamp());
            if (!cancelRequest.Success)
            {
                return BadRequest("An error occurred while creating request into Blockchain!");
            }

            await Task.Delay(_configuration.Value.BlockTime);

            var receiptResponse = await _blockchainApi.GetReceipt(cancelRequest.TransactionId);
            if (!receiptResponse.Success)
                return BadRequest(receiptResponse.Error.Parse());

            return Success(new { }, "Request is canceled successfully");
        }

        [HttpPut("{requestGuid}/pay")]
        public async Task<IActionResult> PayRequest(Guid requestGuid)
        {
            var request = await _paymentRequestService.GetRequestByGuid(requestGuid);
            if (request == null)
                return BadRequest("Something went wrong");

            var changePaymentRequest = new ChangePaymentRequestStatus()
            {
                Id = request.Id,
                PaymentStatus = PaymentStatus.Paid,
                PaymentTimeUtc = DateTime.UtcNow
            };

            var isUpdated = await _paymentRequestService.ChangeStatus(changePaymentRequest);
            if (isUpdated == 0)
                return BadRequest("Something went wrong");

            var payRequest = await _blockchainApi.PayRequest(request.Id, User.GetUserWalletAddress(), DateTime.UtcNow.ConvertToUnixTimestamp());
            if (!payRequest.Success)
            {
                return BadRequest("An error occurred in Blockchain payment method.");
            }

            await Task.Delay(_configuration.Value.BlockTime);

            var receiptResponse = await _blockchainApi.GetReceipt(payRequest.TransactionId);
            if (!receiptResponse.Success)
                return BadRequest(ErrorParser.Parse(receiptResponse.Error));

            return Success(new { }, "Request is successfully paid.");
        }
    }
}