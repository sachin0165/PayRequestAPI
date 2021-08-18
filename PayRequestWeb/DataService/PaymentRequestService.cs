using Dapper;
using PayRequestWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PayRequestWeb.DataService
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly IDatabaseSettings _databaseSettings;

        public PaymentRequestService(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<int> CreateRequest(CreateRequest createRequest)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@RequestGuid", createRequest.RequestGuid);
                    parameter.Add("@Reason", createRequest.Reason);
                    parameter.Add("@FromAddress", createRequest.FromAddress);
                    parameter.Add("@ToAddress", createRequest.ToAddress);
                    parameter.Add("@Amount", createRequest.Amount);
                    parameter.Add("@AmountInSatoshi", createRequest.AmountInSatoshi);
                    parameter.Add("@PaymentStatus", (int)createRequest.PaymentStatus);
                    parameter.Add("@Expiry", createRequest.Expiry);

                    var result = await con.QueryFirstOrDefaultAsync<int>("CreateRequest", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CreateRequest>> GetAllRequests(string walletAddress)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@WalletAddress", walletAddress);

                    var result = await con.QueryAsync<CreateRequest>("GetAllRequests", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CreateRequest> GetRequestByGuid(Guid requestGuid)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@RequestGuid", requestGuid);

                    var result = await con.QueryFirstOrDefaultAsync<CreateRequest>("GetRequestByGuid", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ChangeStatus(ChangePaymentRequestStatus request)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@Id", request.Id);
                    parameter.Add("@PaymentStatus", (int)request.PaymentStatus);
                    parameter.Add("@CancellationTimeUtc", request.CancellationTimeUtc);
                    parameter.Add("@PaymentTimeUtc", request.PaymentTimeUtc);

                    var result = await con.QueryFirstOrDefaultAsync<int>("ChangeRequestStatus", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
