import stateManager from "./state-manager.js";
import statePersister from "./state-persister.js";

const FIVE_MINUTES = 5 * 60 * 1000;

class ChangeManager {
  getOrCreateLatestChange(state, type, path) {
    let change = null;

    for (let c of state.changes) {
      if (c['$type'] == type && path == c.path) {
        change = c;
        continue;
      }
      if (c['$type'] == 'blocktype' && path.indexOf(`${c.path}.`) == 0) {
        change = null;
        continue;
      }
      if (c['$type'] == 'simple' && c.path.indexOf(`${path}.`) == 0) {
        change = null;
        continue;
      }
    }

    if (!change || Date.now() - change.date > FIVE_MINUTES) {
      change = { '$type': type, 'date': Date.now(), path };
      state.changes.push(change);
    }

    return change;
  }

  discardChanges(entityReference) {
    const state = stateManager.getState(entityReference);

    state.changes.splice(0, state.changes.length);

    statePersister.persist(state);
  }

  hasChanges(state) {
    if (state.changes == null) {
      return false;
    }

    return state.changes.length;
  }

  getMergedChanges(state) {
    if (state.changes == null) {
      return [];
    }

    const changes = {};

    for (let change of state.changes) {
      if (change.$type == 'blocktype') {
        Object.keys(changes).filter(path => path.indexOf(`${change.path}.`) == 0).forEach(path => delete changes[path]);
      }

      changes[change.path] = change;
    }

    Object.values(changes).filter(change => change.$type == 'simple').filter(change => change.value == this.getSourceValue(state.source.value, change.path)).forEach(change => delete changes[change.path])
    Object.values(changes).filter(change => change.$type == 'blocktype').filter(change => {
      const sourceValue = this.getSourceValue(state.source.value, change.path);

      return (change.type == null && sourceValue == null) || (sourceValue != null && change.type == sourceValue.Type);
    }).forEach(change => delete changes[change.path])

    return Object.values(changes);
  }

  getSourceValue(value, path) {
    let pathSegments = path.split('.');

    while (pathSegments.length) {
      if (!value) {
        return null;
      }

      if (pathSegments.length > 1) {
        value = value[pathSegments[0]] ? value[pathSegments[0]].Value : null;
      } else {
        value = value[pathSegments[0]];
      }

      pathSegments = pathSegments.splice(1); // returns tail, mutated array discarded
    }

    return value;
  }
}

export default new ChangeManager();