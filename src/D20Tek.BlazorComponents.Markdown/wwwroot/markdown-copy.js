export function init(root) {
    if (root.__markdownCopyInit) return;
    root.__markdownCopyInit = true;

    root.addEventListener("click", async (e) => {
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
    });
}

export function dispose(root) {
    if (root) root.__markdownCopyInit = false;
}
