using D20Tek.BlazorComponents;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ToggleSwitchPage
{
    private ToggleSwitch? _ref;
    private string _interactiveLabelText = "Label";
    private string _interactiveColor = string.Empty;
    private Size _interactiveSize = Size.Medium;
    private bool _interactiveVisibility = true;
    private bool _toggleVisible = true;
    private string _displayMessage = string.Empty;

    private string ToggleLabel => _interactiveVisibility ? "(visible)" : "(hidden)";

    private void ToggleSwitchVisibility() => _toggleVisible = !_toggleVisible;

    private void ToggleCheckChanged(bool newValue) => _displayMessage = $"ToggleSwitch check changed: {newValue}";
}
