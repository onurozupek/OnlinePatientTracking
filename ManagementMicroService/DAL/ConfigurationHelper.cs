namespace ManagementMicroservice.DAL
{
    public static class ConfigurationHelper
    {
        public static string GetConnectionString(IConfiguration configuration, string name)
        {
            return configuration.GetConnectionString(name);
        }
    }
}
