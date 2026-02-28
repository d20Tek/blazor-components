export function showModal(dialogId) {
    const dialog = document.getElementById(dialogId);
    if (dialog && typeof dialog.showModal === 'function') {
        dialog.showModal();
    }
}

export function closeModal(dialogId) {
    const dialog = document.getElementById(dialogId);
    if (dialog && typeof dialog.close === 'function') {
        dialog.close();
    }
}
