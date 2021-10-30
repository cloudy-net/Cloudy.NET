class FormEventDispatcher {
    onChangeCallbacks = [];

    triggerChange() {
        this.onChangeCallbacks.forEach(callback => callback(...arguments));
    }
    onChange(callback) {
        this.onChangeCallbacks.push(callback);
    }
}

export default FormEventDispatcher;