using System;

namespace PayRequestWeb.Helpers
{
    public static class ErrorParser
    {
        public static string Parse(this string errorString)
        {
            try
            {
                var pFrom = errorString.IndexOf("SmartContractAssertException:", StringComparison.Ordinal) + "SmartContractAssertException: ".Length;
                var pTo = errorString.LastIndexOf("at Stratis.SmartContracts.SmartContract ", StringComparison.Ordinal);

                string result = errorString.Substring(pFrom, pTo - pFrom);
                return result;
            }
            catch
            {
                return errorString;
            }
        }
    }
}
