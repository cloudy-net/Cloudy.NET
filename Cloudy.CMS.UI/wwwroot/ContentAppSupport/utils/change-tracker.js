import contentSaver from "./content-saver.js";
import arrayEquals from "./array-equals.js";
import contentGetter from "./content-getter.js";

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

        let changesForContent = this.pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!changesForContent) {
            changesForContent = { contentId, contentTypeId, changedFields: [] };
            this.pendingChanges.push(changesForContent);
        }

        if (change.remove === true) {
            changesForContent.remove = true;
        }
        if (change.remove === false) {
            delete changesForContent.remove;
        }

        let changedField = changesForContent.changedFields.find(f => arrayEquals(path, f.path));

        if (change.fieldType == 'simple') {
            if (!changedField) {
                changesForContent.changedFields.push(changedField = { path, type: 'simple', initialValue: change.initialValue, value: change.value });
            }

            if (change.type == 'set') {
                if (change.initialValue === change.value) {
                    changesForContent.changedFields.splice(changesForContent.changedFields.indexOf(changedField), 1); // delete unchanged field
                } else {
                    changedField.value = change.value;
                }
            }
        }
        
        if (change.fieldType == 'array') {
            if (!changedField) {
                changesForContent.changedFields.push(changedField = { path, type: 'array', changes: [] });
            }

            if (change.type == 'add') {
                changedField.changes.push({ id: change.id, type: change.add, value: JSON.stringify(change.value) });
            }
            if (change.type == 'update') {
                const item = changedField.changes.find(i => i.id == change.id);
                item.value = change.value;
            }
            if (change.type == 'delete') {
                var item = changedField.changes.find(i => i.id == change.id);

                if (item.type == 'add') {
                    changedField.changes.splice(changedField.changes.indexOf(item), 1); // delete addition completely
                } else {
                    item.type = 'delete';
                    delete item.value;
                }
            }
        }

        if (changesForContent.changedFields.length == 0 && !changesForContent.remove) {
            this.pendingChanges.splice(this.pendingChanges.indexOf(changesForContent), 1); // delete empty change object
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
                remove: c.remove,
                changedFields: c.changedFields
            }
        });
        if (await contentSaver.save(contentToSave) == false) {
            return false; // fail
        }
        _pendingChanges.forEach(c => contentGetter.clearCacheFor(c.contentId, c.contentTypeId));
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