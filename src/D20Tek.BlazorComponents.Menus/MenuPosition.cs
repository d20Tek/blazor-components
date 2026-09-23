namespace D20Tek.BlazorComponents;

public readonly record struct MenuPosition(
    double X,
    double Y,
    MenuPlacement ResolvedPlacement);
