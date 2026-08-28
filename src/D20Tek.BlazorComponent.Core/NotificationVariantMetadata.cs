namespace D20Tek.BlazorComponents;

public class NotificationVariantMetadata
{
    public class Item(string token, string defaultIcon)
    {
        public string Token { get; } = token;

        public string DefaultIcon { get; } = defaultIcon;
    }

    private static readonly Dictionary<NotificationVariant, Item> _elements = new()
    {
        { NotificationVariant.Info, new Item("info", InfoIcon) },
        { NotificationVariant.Success, new Item("success", SuccessIcon) },
        { NotificationVariant.Warning, new Item("warning", WarningIcon) },
        { NotificationVariant.Error, new Item("error", ErrorIcon) },
        { NotificationVariant.Neutral, new Item("neutral", InfoIcon) },
    };

    public static Item GetMetadataItem(NotificationVariant variant) => _elements[variant];

    public static string GetVariantToken(NotificationVariant variant) => _elements[variant].Token;

    public static string GetDefaultIcon(NotificationVariant variant) => _elements[variant].DefaultIcon;

    private const string InfoIcon = """
        <svg viewBox="0 0 24 24" class="result-alert__icon--info" aria-hidden="true">
            <circle cx="12" cy="12" r="10" fill="#3b82f6"/>
            <text x="12" y="16" text-anchor="middle" fill="white" font-size="14" font-weight="bold">i</text>
        </svg>
        """;

    private const string SuccessIcon = """
        <svg viewBox="0 0 24 24" class="result-alert__icon--success" aria-hidden="true">
            <circle cx="12" cy="12" r="10" fill="#22c55e"/>
            <path d="M8 12l2 2 4-4" stroke="white" stroke-width="2" fill="none"/>
        </svg>
        """;

    private const string WarningIcon = """
        <svg viewBox="0 0 24 24" class="result-alert__icon--warning" aria-hidden="true">
            <path d="M12 2L2 22h20L12 2z" fill="#f59e0b"/>
            <text x="12" y="18" text-anchor="middle" fill="white" font-size="12" font-weight="bold">!</text>
        </svg>
        """;

    private const string ErrorIcon = """
        <svg viewBox="0 0 24 24" class="result-alert__icon--error" aria-hidden="true">
            <circle cx="12" cy="12" r="10" fill="#ef4444"/>
            <path d="M8 8l8 8M16 8l-8 8" stroke="white" stroke-width="2"/>
        </svg>
        """;
}
