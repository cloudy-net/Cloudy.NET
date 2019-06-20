


/* REMOVE ELEMENT LISTENER */

class RemoveElementListener {
    constructor(element, callback) {
        var observer = new MutationObserver(mutations => mutations.forEach(mutation => {
            mutation.removedNodes.forEach(node => {
                if (node != element && !node.contains(element)) {
                    return;
                }

                callback();
                observer.disconnect();
            });
        }));

        observer.observe(document.documentElement, {
            childList: true,
            subtree: true,
        });
    }
}

export default RemoveElementListener;