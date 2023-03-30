using Microsoft.JSInterop;

namespace Pokewordle.Shared.Services; 

public class SettingsService
{
    private readonly IJSRuntime _jsRuntime;

    public SettingsService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetSettingAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task SetSettingAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }
}