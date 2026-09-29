using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.FullSample.Wasm.Shared;

public partial class ThemeToggle
{
    private bool _isDark;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var current = await JS.InvokeAsync<string>("d20tekTheme.get");
            _isDark = current == "dark";
            StateHasChanged();
        }
    }

    private async Task ToggleAsync()
    {
        _isDark = !_isDark;
        await JS.InvokeAsync<string>("d20tekTheme.set", _isDark ? "dark" : "light");
    }
}
