using Cart.Application.Clients.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Application.Clients;

public class WandServiceClient : IWandServiceClient
{
    private readonly HttpClient _httpClient;

    public WandServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsWandValidAsync(int wandId)
    {
        var response = await _httpClient.GetAsync("/api/Wand/valid");
        if (!response.IsSuccessStatusCode)
            return false;

        var validWands = await response.Content.ReadFromJsonAsync<List<WandDto>>();
        if (validWands == null)
            return false;

        return validWands.Any(w => w.Id == wandId);
    }

    public async Task<WandDto?> GetWandByIdAsync(int wandId)
    {
        var response = await _httpClient.GetAsync($"/api/Wand/{wandId}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<WandDto>();
    }
}

