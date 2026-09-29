// Simple theme manager for the sample app. Persists the selected theme in
// localStorage and applies it to <html> via data-theme + color-scheme so that
// the D20Tek components (which opt into dark via the data-theme marker and the
// host color-scheme) render in the chosen theme.
(function () {
    const storageKey = 'd20tek-sample-theme';
    const root = document.documentElement;

    function apply(theme) {
        // theme is 'light' or 'dark'.
        root.setAttribute('data-theme', theme);
        root.style.colorScheme = theme;
    }

    // Apply as early as possible to avoid a flash of the wrong theme.
    const saved = localStorage.getItem(storageKey);
    apply(saved === 'dark' ? 'dark' : 'light');

    window.d20tekTheme = {
        get: function () {
            return root.getAttribute('data-theme') === 'dark' ? 'dark' : 'light';
        },
        set: function (theme) {
            const normalized = theme === 'dark' ? 'dark' : 'light';
            apply(normalized);
            localStorage.setItem(storageKey, normalized);
            return normalized;
        }
    };
})();
