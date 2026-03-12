using D20Tek.BlazorComponents;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class TogglePanelPage
{
    private string _summary = "Interactive Panel";
    private Size _size = Size.Medium;
    private bool _isOpen = false;
    private bool _showChevron = true;
    private bool _isVisible = true;
    private string _lastToggle = "None";
    private bool _boundIsOpen = false;

    private void HandleToggle(bool isOpen) => _lastToggle = isOpen ? "Opened" : "Closed";
}
