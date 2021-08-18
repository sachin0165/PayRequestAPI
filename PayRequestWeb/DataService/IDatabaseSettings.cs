using Microsoft.Extensions.Configuration;

namespace PayRequestWeb.DataService
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public DatabaseSettings(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public string ConnectionString { get; private set; }
    }
}
