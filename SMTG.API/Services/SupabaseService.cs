using Supabase;

namespace SMTG.API.Services;

public class SupabaseService
{
    public Client Client { get; }

    public SupabaseService(IConfiguration configuration)
    {
        var url = configuration["Supabase:Url"]!;
        var key = configuration["Supabase:AnonKey"]!;

        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        Client = new Client(url, key, options);
        Client.InitializeAsync().Wait();
    }
}