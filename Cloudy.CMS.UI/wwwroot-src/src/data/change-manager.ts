import Change from "./change";
import State from "./state";

const FIVE_MINUTES = 5 * 60 * 1000;

class ChangeManager {
  getOrCreateLatestChange(state: State, type: string, path: string) {
    let change: Change | null = null;

    for (let c of state.history) {
      if (c.$type == type && path == c.path) {
        change = c;
        continue;
      }
      if (c.$type == 'blocktype' && path.indexOf(`${c.path}.`) == 0) {
        change = null;
        continue;
      }
      if (c.$type == 'simple' && c.path.indexOf(`${path}.`) == 0) {
        change = null;
        continue;
      }
    }

    if (!change || ((type != "simple" && type != "blocktype") || Date.now() - change.date > FIVE_MINUTES)) {
      change = { '$type': type, 'date': Date.now(), path };
      state.history.push(change!);
    }

    return change;
  }

  discardChanges(state: State) {
    state.history = [];
    state.changes = [];
  }

  getChanges(state: State) {
    if (state.history == null) {
      return [];
    }

    const changes: Change[] = [];

    // do these operations in a for loop so that any operations only happen to previous changes
    for (let change of state.history) {

      // clear nested property changes when changing block type
      if (change.$type == 'blocktype') {
        changes
          .filter(c => c.path.indexOf(`${change.path}.`) == 0)
          .forEach(c => changes.splice(changes.indexOf(c), 1));
      }

      // clear nested property changes when deleting block in list
      if (change.$type == 'embeddedblocklist.remove') {
        changes
          .filter(c => c.path.indexOf(`${change.path}.`) == 0)
          .forEach(c => changes.splice(changes.indexOf(c), 1));
      }

      // only allow 1 change per path for simple and blocktype changes
      if (change.$type == "simple" || change.$type == "blocktype") {
        changes
          .filter(c => c.path == change.path)
          .forEach(c => changes.splice(changes.indexOf(c), 1));
      }

      // if removing block that is newly added, remove both changes
      if (change.$type == 'embeddedblocklist.remove') {
        const newlyAdded = changes.find(c => c.path == change.path && c.$type == "embeddedblocklist.add" && c.key == change.key);

        if (newlyAdded) {
          changes.splice(changes.indexOf(newlyAdded), 1); // remove "add" change
          continue; // skip adding "remove" change
        }
      }

      changes.push(change);
    }

    // clear simple changes where value matches source
    changes
      .filter(change => change.$type == 'simple')
      .filter(change => state.source && (change.value == this.getSourceValue(state.source.value, change.path)))
      .forEach(c => changes.splice(changes.indexOf(c), 1));

    // clear blocktype changes where type matches source type
    changes
      .filter(change => change.$type == 'blocktype')
      .filter(change => {
        const sourceValue = this.getSourceValue(state.source.value, change.path);

        return (change.type == null && sourceValue == null) || (sourceValue != null && change.type == sourceValue.Type);
      })
      .forEach(c => changes.splice(changes.indexOf(c), 1));

    return changes;
  }

  getSourceValue(value: any, path: string) {
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