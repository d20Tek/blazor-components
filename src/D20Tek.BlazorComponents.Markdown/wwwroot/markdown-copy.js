let _handler = null;
let _refCount = 0;

export function init() {
    _refCount++;
    if (_handler) return;

    _handler = async (e) => {
        const btn = e.target.closest(".markdown-copy-btn");
        if (!btn) return;

        const codeEl = btn.closest(".markdown-code-block")?.querySelector("pre > code");
        if (!codeEl) return;

        try {
            await navigator.clipboard.writeText(codeEl.textContent);
            btn.classList.add("copied");
            btn.setAttribute("title", "Copied!");
            setTimeout(() => {
                btn.classList.remove("copied");
                btn.setAttribute("title", "Copy code");
            }, 2000);
        } catch {
            // clipboard API not available or permission denied — fail silently
        }
    };

    document.addEventListener("click", _handler);
}

export function dispose() {
    _refCount = Math.max(0, _refCount - 1);
    if (_refCount === 0 && _handler) {
        document.removeEventListener("click", _handler);
        _handler = null;
    }
}

