using ABCRetailDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Load Azure Storage connection string from environment variable first
string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING") 
                          ?? builder.Configuration.GetConnectionString("AzureStorage");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Azure Storage connection string is not set. Please set AZURE_STORAGE_CONNECTION_STRING environment variable.");
}

// Register services with the connection string
builder.Services.AddSingleton(sp => new TableService(connectionString));
builder.Services.AddSingleton(sp => new BlobService(connectionString, "productimages"));
builder.Services.AddSingleton(sp => new QueueService(connectionString, "orders"));
builder.Services.AddSingleton(sp => new FileService(connectionString, "orderlogs"));

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
