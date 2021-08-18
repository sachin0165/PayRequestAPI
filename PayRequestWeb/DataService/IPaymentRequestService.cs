using PayRequestWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayRequestWeb.DataService
{
    public interface IPaymentRequestService
    {
        Task<int> CreateRequest(CreateRequest createRequest);

        Task<List<CreateRequest>> GetAllRequests(string walletAddress);

        Task<CreateRequest> GetRequestByGuid(Guid requestGuid);

        Task<int> ChangeStatus(ChangePaymentRequestStatus request);
    }
}
