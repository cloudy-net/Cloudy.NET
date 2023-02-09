const FIVE_MINUTES = 5 * 60 * 1000;

class ChangeManager {
  getOrCreateLatestChange(state, type, path) {
    let change = null;

    for (let c of state.history) {
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
      state.history.push(change);
    }

    return change;
  }

  discardChanges(state) {
    state.history = [];
    state.changes = [];
  }

  hasChanges(state) {
    if (state.history == null) {
      return false;
    }

    return state.history.length;
  }

  getChanges(state) {
    if (state.history == null) {
      return [];
    }

    const history = {};

    for (let change of state.history) {
      if (change.$type == 'blocktype') {
        Object.keys(history).filter(path => path.indexOf(`${change.path}.`) == 0).forEach(path => delete history[path]);
      }

      history[change.path] = change;
    }

    Object.values(history).filter(change => change.$type == 'simple').filter(change => change.value == this.getSourceValue(state.source.value, change.path)).forEach(change => delete history[change.path])
    Object.values(history).filter(change => change.$type == 'blocktype').filter(change => {
      const sourceValue = this.getSourceValue(state.source.value, change.path);

      return (change.type == null && sourceValue == null) || (sourceValue != null && change.type == sourceValue.Type);
    }).forEach(change => delete history[change.path])

    return Object.values(history);
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