export default element => {
    element.dispatchEvent(new Event('close-dropdown', { bubbles: true }));
};