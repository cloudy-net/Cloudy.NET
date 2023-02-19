class StateEvents {
  _onEntityReferenceChangeCallbacks = [];

  onEntityReferenceChange(callback) {
    this._onEntityReferenceChangeCallbacks.push(callback);
  }

  offEntityReferenceChange(callback) {
    this._onEntityReferenceChangeCallbacks.splice(this._onEntityReferenceChangeCallbacks.indexOf(callback), 1);
  }

  triggerEntityReferenceChange(entityReference) {
    this._onEntityReferenceChangeCallbacks.forEach(callback => callback(entityReference));
  }
  
  _onStateChangeCallbacks = [];

  onStateChange(callback) {
    this._onStateChangeCallbacks.push(callback);
  }

  offStateChange(callback) {
    this._onStateChangeCallbacks.splice(this._onStateChangeCallbacks.indexOf(callback), 1);
  }

  triggerStateChange(state) {
    this._onStateChangeCallbacks.forEach(callback => callback(state));
  }
}

export default new StateEvents();