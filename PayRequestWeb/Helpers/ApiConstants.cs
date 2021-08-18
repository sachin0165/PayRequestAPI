using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayRequestWeb.Helpers
{
    public class ApiConstants
    {
        public const string JwtRegisteredClaimNamesUserId = "uid";
        public const string JwtRegisteredClaimNamesUserWalletAddress = "uwa";

        public const long OneSatoshi = 100_000_000;
    }
}
