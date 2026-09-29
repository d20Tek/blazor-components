using D20Tek.BlazorComponents;
using Microsoft.AspNetCore.Components;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class FlyoutMenusPage
{
    private MenuPlacement _placement = MenuPlacement.BottomEnd;
    private Size _size = Size.Small;
    private int _offset = 6;
    private bool _flipEnabled = true;
    private bool _shiftEnabled = true;
    private bool _closeOnScroll;
    private bool _disabled;

    private string _lastAction = "(none)";

    private IReadOnlyList<IMenuEntry> _items = default!;

    protected override void OnInitialized() =>
        _items =
        [
            new MenuHeaderItem { Label = "Actions" },
            new MenuActionItem
            {
                Label = "Edit",
                Icon = "oi oi-pencil",
                OnClick = EventCallback.Factory.Create(this, () => Log("edit")),
            },
            new MenuActionItem
            {
                Label = "Share",
                Icon = "oi oi-share",
                OnClick = EventCallback.Factory.Create(this, () => Log("share")),
            },
            new MenuLinkItem
            {
                Label = "View docs",
                Icon = "oi oi-book",
                Href = "https://components.d20tek.com",
                Target = "_blank",
            },
            new MenuSeparatorItem(),
            new MenuActionItem
            {
                Label = "Delete",
                Icon = "oi oi-trash",
                IsDestructive = true,
                OnClick = EventCallback.Factory.Create(this, () => Log("delete")),
            },
        ];

    private void Log(string action)
    {
        _lastAction = $"{action} @ {DateTime.Now:HH:mm:ss}";
        StateHasChanged();
    }
}
