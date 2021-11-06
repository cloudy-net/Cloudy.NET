import contentSaver from "./content-saver.js";
import arrayEquals from "./array-equals.js";

/* CHANGE TRACKER */

class ChangeTracker {
    pendingChanges = JSON.parse(localStorage.getItem('_pendingChanges')) || [];
    persistPendingChanges() {
        localStorage.setItem('_pendingChanges', JSON.stringify(this.pendingChanges));
    }

    onUpdateCallbacks = [];
    onUpdate(callback) {
        this.onUpdateCallbacks.push(callback);
        callback();
    }
    triggerUpdate() {
        this.onUpdateCallbacks.forEach(callback => callback());
    }
    removeOnUpdate(callback) {
        const index = this.onUpdateCallbacks.indexOf(callback);

        if (index == -1) {
            return;
        }

        this.onUpdateCallbacks.splice(index, 1);
    }

    referenceObjects = [];
    getReferenceObject(contentId, contentTypeId) {
        const value = this.referenceObjects.find(r => arrayEquals(r.contentId, contentId) && contentTypeId == r.contentTypeId);

        if (!value) {
            return; // returns undefined
        }

        return value.content;
    }
    setReferenceObject(content, contentId, contentTypeId) {
        var referenceObject = this.getReferenceObject(contentId, contentTypeId);

        if (referenceObject) {
            this.referenceObjects.splice(this.referenceObjects.indexOf(referenceObject), 1); // delete reference object
        }

        this.referenceObjects.push({ content, contentId, contentTypeId });
    }

    addChange(contentId, contentTypeId, path, change) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        //const referenceObject = this.getReferenceObject(contentId, contentTypeId);
        let pendingChange = this.pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!pendingChange) {
            this.pendingChanges.push(pendingChange = { contentId, contentTypeId, changedFields: [] });
        }

        let changedField = pendingChange.changedFields.find(f => arrayEquals(path, f.path));

        if (change.type == 'change') {
            if (!changedField) {
                pendingChange.changedFields.push(changedField = { path, initialValue: change.initialValue, value: change.value });
            }

            if (change.initialValue === change.value) {
                pendingChange.changedFields.splice(pendingChange.changedFields.indexOf(changedField), 1); // delete unchanged field
            } else {
                changedField.value = change.value;
            }
        }
        
        // TODO: additions, removals, moves, changes
        if (change.type.indexOf('array.') == 0) {
            if (!changedField) {
                pendingChange.changedFields.push(changedField = { path, changes: [] });
            }
        }
        if (change.type == 'array.add') {
            changedField.changes.push(change);
        }
        if (change.type == 'array.update') {
            const item = changedField.changes.find(i => i.id == change.id);
            item.value = change.value;
        }
        if (change.type == 'array.delete') {
            var item = changedField.changes.find(i => i.id == change.id);

            if (item.type == 'array.add') {
                changedField.changes.splice(changedField.changes.indexOf(item), 1); // delete change
            } else {
                item.type = 'array.delete';
                delete item.value;
            }
        }

        if (pendingChange.changedFields.length == 0) {
            this.pendingChanges.splice(this.pendingChanges.indexOf(pendingChange), 1); // delete empty change object
        }

        this.persistPendingChanges();
        this.triggerUpdate();
    }

    getFor(contentId, contentTypeId) {
        return this.pendingChanges.find(p => arrayEquals(p.contentId, contentId) && p.contentTypeId == contentTypeId);
    }

    reset(contentId, contentTypeId) {
        const _pendingChanges = this.pendingChanges;
        const index = _pendingChanges.findIndex(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);
        if (index !== -1) {
            _pendingChanges.splice(index, 1);
            this.persistPendingChanges();
            this.triggerUpdate();
        }
    }

    async applyFor(contentId, contentTypeId) {
        await this.apply([this.getFor(contentId, contentTypeId)]);
    }

    async apply(pendingChanges, callBack) {
        const _pendingChanges = pendingChanges || this.pendingChanges;
        if (!_pendingChanges.length) {
            return;
        }
        const contentToSave = _pendingChanges.map(c => {
            return {
                keyValues: c.contentId,
                contentTypeId: c.contentTypeId,
                changedFields: c.changedFields
            }
        })
        if (await contentSaver.save(contentToSave) == false) {
            return false; // fail
        }
        const _remainingPendingChanges = [];
        this.pendingChanges.forEach(c => {
            if (!_pendingChanges.some(d => arrayEquals(d.contentId, c.contentId) && d.contentTypeId === c.contentTypeId)) {
                _remainingPendingChanges.push(c);
            }
        })
        this.pendingChanges = _remainingPendingChanges;
        this.persistPendingChanges();
        this.triggerUpdate();
        callBack && callBack();
    }

    getPendingValue(contentId, contentTypeId, path, value) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const changesForContent = this.pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!changesForContent) {
            return value;
        }

        const changedField = changesForContent.changedFields.find(c => arrayEquals(path, c.path));

        if (!changedField) {
            return value;
        }

        return changedField.value;
    }

    mergeWithPendingChanges(contentId, contentTypeId, content) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)');
        }

        const changesForContent = this.pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        const contentMapping = { ...content };

        if (!changesForContent) {
            return contentMapping;
        }

        for (const changedField of changesForContent.changedFields) {
            if (changedField.value) {
                contentMapping[changedField.name] = changedField.value;
            }
            if (changedField.changes) {
                if (!Array.isArray(contentMapping[changedField.name])) {
                    contentMapping[changedField.name] = [];
                }
                for (const change of changedField.changes.filter(c => c.type == 'array.add')) {
                    contentMapping[changedField.name].push(change.value);
                }
            }
        }

        return contentMapping;
    }
}

export default new ChangeTracker();