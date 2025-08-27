using Azure.Data.Tables;
using System;

namespace ABCRetailDemo.Services
{
    public static class ConnectionStringValidator
    {
        public static void Validate(string connectionString)
        {
            try
            {
                // Try creating a TableServiceClient
                var client = new TableServiceClient(connectionString);
                
                // Optional: Try listing tables (will throw if key is invalid)
                var tables = client.QueryAsync().AsPages();
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException(
                    "Invalid Azure Storage connection string. Make sure the AccountKey is copied exactly as shown in the portal.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Failed to connect to Azure Storage. Check your connection string and network.", ex);
            }
        }
    }
}
