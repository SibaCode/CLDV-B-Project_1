public FileService(IConfiguration config)
{
    var connectionString = config["AzureFiles:ConnectionString"];
    var shareName = config["AzureFiles:ShareName"];

    if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(shareName))
        throw new ArgumentNullException("AzureFiles connection string or share name is missing.");

    _shareClient = new ShareClient(connectionString, shareName);
    _shareClient.CreateIfNotExists();
}
