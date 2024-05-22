
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.http;

public class CommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public  async Task SendPlatformToCommand(PlatformCreateDto platform)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", content);
        if(response.IsSuccessStatusCode)
        {
            Console.WriteLine("Sync post to command service is Ok");
        }
        else
        {
             Console.WriteLine("Sync post to command service is NOT Ok");
        }
    }
}
