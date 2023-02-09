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

  getSourceConflicts(state, mergedChanges) {
    if (!state.newSource) {
      return [];
    }

    const conflicts = [];

    for (let key of Object.keys(state.source.properties)) {
      if (!state.newSource.properties[key] && mergedChanges.find(change => change.path == key)) {
        conflicts.push({ path: key, type: 'deleted' });
      }
    }

    const sourceBlockTypes = this.getSourceBlockTypes(state.source.value);
    const newSourceBlockTypes = this.getSourceBlockTypes(state.newSource.value);

    for (let path of Object.keys(sourceBlockTypes)) {
      if (!newSourceBlockTypes[path] || sourceBlockTypes[path] != newSourceBlockTypes[path]) {
        for (let change of mergedChanges.filter(change => change.path.indexOf(`${path}.`) == 0)) {
          conflicts.push({ path: change.path, type: 'blockdeleted' });
        }
      }
    }

    const newSourceProperties = this.enumerateSourceProperties(state.source.value);

    for (let path of newSourceProperties) {
      const newSourceValue = this.getSourceValue(state.newSource.value, path);
      const sourceValue = this.getSourceValue(state.source.value, path);

      if (newSourceValue == sourceValue) {
        continue;
      }

      if (Array.isArray(newSourceValue) && Array.isArray(sourceValue) && arrayEquals(newSourceValue, sourceValue)) {
        continue;
      }

      if (!mergedChanges.find(change => change.path == path)) {
        continue;
      }

      if (conflicts.find(conflict => conflict.path == path)) {
        continue;
      }

      conflicts.push({ path: path, type: 'pendingchangesourceconflict' });
    }

    return conflicts;
  }

  getSourceBlockTypes(source) {
    const cue = [{ target: source, path: '' }];
    const result = {};

    while (cue.length) {
      const { target, path } = cue.shift();

      for (let key of Object.keys(target)) {
        if (!target[key]) {
          continue;
        }

        if (!target[key].Type) {
          continue;
        }

        const currentPath = path + (path ? '.' : '') + key;

        result[currentPath] = target[key].Type;

        if (!target[key].Value) {
          continue;
        }

        cue.push({ target: target[key].Value, path: currentPath });
      }
    }

    return result;
  }

  enumerateSourceProperties(source) {
    const cue = [{ target: source, path: '' }];
    const result = [];

    while (cue.length) {
      const { target, path } = cue.shift();

      for (let key of Object.keys(target)) {
        const currentPath = path + (path ? '.' : '') + key;

        if (!target[key]) {
          result.push(currentPath);
          continue;
        }

        if (!target[key].Type) {
          result.push(currentPath);
          continue;
        }

        if (!target[key].Value) {
          continue;
        }

        cue.push({ target: target[key].Value, path: currentPath });
      }
    }

    return result;
  }

  discardSourceConflicts(state, conflicts, actions) {
    const changes = [...state.changes];

    for (let path of Object.keys(actions)) {
      const action = actions[path];

      if (action != 'keep-source') {
        continue;
      }

      for(let change of this.getAllChangesForPath(changes, path)){
        changes.splice(changes.indexOf(change), 1);
      }
    }

    for(let conflict of conflicts) {
      if(conflict.type != 'blockdeleted' && conflict.type != 'deleted'){
        continue;
      }

      for(let change of this.getAllChangesForPath(changes, conflict.path)){
        changes.splice(changes.indexOf(change), 1);
      }
    }

    state = {
      ...state,
      changes,
      source: state.newSource,
      newSource: null,
    };

    stateManager.replace(state);
  }

  getAllChangesForPath(changes, path) {
    let result = [];

    for (let c of changes) {
      if (c['$type'] == 'blocktype' && path.indexOf(`${c.path}.`) == 0) {
        result = [];
        continue;
      }
      if (path == c.path) {
        result.push(c);
        continue;
      }
    }

    return result;
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