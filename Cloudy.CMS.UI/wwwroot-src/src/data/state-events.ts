import EntityReference from "./entity-reference";
import EntityReferenceCallback from "./entity-reference-callback";
import State from "./state";
import StateChangeCallback from "./state-change-callback";

class StateEvents {
  _onEntityReferenceChangeCallbacks: EntityReferenceCallback[] = [];

  onEntityReferenceChange(callback: EntityReferenceCallback) {
    this._onEntityReferenceChangeCallbacks.push(callback);
  }

  offEntityReferenceChange(callback: EntityReferenceCallback) {
    this._onEntityReferenceChangeCallbacks.splice(this._onEntityReferenceChangeCallbacks.indexOf(callback), 1);
  }

  triggerEntityReferenceChange(entityReference: EntityReference) {
    this._onEntityReferenceChangeCallbacks.forEach(callback => callback(entityReference));
  }
  
  _onStateChangeCallbacks: StateChangeCallback[] = [];

  onStateChange(callback: StateChangeCallback) {
    this._onStateChangeCallbacks.push(callback);
  }

  offStateChange(callback: StateChangeCallback) {
    this._onStateChangeCallbacks.splice(this._onStateChangeCallbacks.indexOf(callback), 1);
  }

  triggerStateChange(state: State | null) {
    this._onStateChangeCallbacks.forEach(callback => callback(state));
  }
}

export default new StateEvents();