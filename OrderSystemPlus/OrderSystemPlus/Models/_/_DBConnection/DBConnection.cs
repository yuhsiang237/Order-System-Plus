namespace OrderSystemPlus.Models
{
    public class DBConnection
    {
        private static IConfiguration _configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                return builder.Build();
            }
        }

        public static string GetConnectionString()
        {
            return _configuration.GetValue<string>("DBSettings:Connection");
        }
    }
}
