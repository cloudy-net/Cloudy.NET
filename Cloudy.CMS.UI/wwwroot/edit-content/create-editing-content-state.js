
function createEditingContentState(contentReference) {
    return {
        ...contentReference,
        values: {}
    };
}

export default createEditingContentState;