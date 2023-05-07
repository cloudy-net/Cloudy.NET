export default (element: any) => {
    const htmlElement = element as HTMLElement;

    if (!htmlElement) {
        return;
    }

    htmlElement.dispatchEvent(new Event('close-dropdown', { bubbles: true }));
};