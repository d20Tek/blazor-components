export function showModal(element) {
    if (element && typeof element.showModal === 'function') {
        element.showModal();
    }
}

export function closeModal(element) {
    if (element && typeof element.close === 'function') {
        element.close();
    }
}
