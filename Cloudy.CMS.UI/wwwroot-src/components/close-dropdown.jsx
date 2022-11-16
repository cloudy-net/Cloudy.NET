export default event => {
    event.target.dispatchEvent(new Event('close-dropdown', { bubbles: true }));
};