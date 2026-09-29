namespace D20Tek.BlazorComponents;

internal readonly record struct MenuPosition(
    double X,
    double Y,
    MenuPlacement ResolvedPlacement);
