class StateEvents {
  _onAnyStateChangeCallbacks = [];

  onAnyStateChange(callback) {
    this._onAnyStateChangeCallbacks.push(callback);
  }

  offAnyStateChange(callback) {
    this._onAnyStateChangeCallbacks.splice(this._onAnyStateChangeCallbacks.indexOf(callback), 1);
  }

  triggerAnyStateChange() {
    this._onAnyStateChangeCallbacks.forEach(callback => callback());
  }

  _onStateChangeCallbacks = {};

  onStateChange(entityReference, callback) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].push(callback);
  }

  offStateChange(entityReference, callback) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].splice(this._onStateChangeCallbacks[key].indexOf(callback), 1);
  }

  triggerStateChange(entityReference) {
    const key = JSON.stringify(entityReference);

    if (!this._onStateChangeCallbacks[key]) {
      this._onStateChangeCallbacks[key] = [];
    }

    this._onStateChangeCallbacks[key].forEach(callback => callback());
  }
}

export default new StateEvents();