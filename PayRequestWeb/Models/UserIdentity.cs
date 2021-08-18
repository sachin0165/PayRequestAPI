using System.Security.Principal;

namespace PayRequestWeb.Models
{
    public class UserIdentity : IIdentity
    {
        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public string WalletAddress { get; set; }
    }
}
