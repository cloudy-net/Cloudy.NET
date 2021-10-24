import contentSaver from "./content-saver.js";
import idEquals from "./utils/id-equals.js";

/* CHANGE TRACKER */

class ChangeTracker {
    _pendingChanges;
    _pendingChangesKey = '_pendingChangesKey';

    set pendingChanges(value) {
        this._pendingChanges = value;
        localStorage.setItem(this._pendingChangesKey, JSON.stringify(value));
    }

    get pendingChanges() {
        return this._pendingChanges || JSON.parse(localStorage.getItem(this._pendingChangesKey)) || [];
    }

    onUpdateCallbacks = [];

    onUpdate(callback) {
        this.onUpdateCallbacks.push(callback);
        callback();
    }

    removeOnUpdate(callback) {
        const index = this.onUpdateCallbacks.indexOf(callback);

        if (index == -1) {
            return;
        }

        this.onUpdateCallbacks.splice(index, 1);
    }

    save(contentId, contentTypeId, contentAsJson) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const { name, value, originalValue } = contentAsJson;
        const _pendingChangeToSave = this.pendingChanges;
        const index = _pendingChangeToSave.findIndex(c => idEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);
        if (index === -1) {
            _pendingChangeToSave.push({
                contentId,
                contentTypeId,
                changedFields: value !== originalValue ? [{...contentAsJson}]: []
            });
            this.update(_pendingChangeToSave);
            return;
        }

        const changeFieldIndex = _pendingChangeToSave[index].changedFields.findIndex(f => f.name === name);
        if (changeFieldIndex === -1 && value !== originalValue) {
            _pendingChangeToSave[index].changedFields.push(contentAsJson);
            this.update(_pendingChangeToSave);
            return;
        }

        if (changeFieldIndex !== -1) {
            if ( value === originalValue) {
                _pendingChangeToSave[index].changedFields.splice(changeFieldIndex, 1);
            } else {
                _pendingChangeToSave[index].changedFields[changeFieldIndex].value = value;
            }
        }
        this.update(_pendingChangeToSave);
    }

    getFor(contentId, contentTypeId) {
        return this.pendingChanges.find(p => idEquals(p.contentId, contentId) && p.contentTypeId == contentTypeId);
    }

    update(pendingChanges = this.pendingChanges) {
        this.pendingChanges = pendingChanges;
        this.onUpdateCallbacks.forEach(callback => callback());
    }

    reset(contentId, contentTypeId) {
        const _pendingChanges = this.pendingChanges;
        const index = _pendingChanges.findIndex(c => idEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);
        if (index !== -1) {
            _pendingChanges.splice(index, 1);
            this.update(_pendingChanges);
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
            const changedArray = c.changedFields.map(f => {
                const { originalValue, ...changedObj } = f;
                return changedObj;
            });
            return {
                keyValues: c.contentId,
                contentTypeId: c.contentTypeId,
                changedFields: changedArray
            }
        })
        if (await contentSaver.save(contentToSave) == false) {
            return false; // fail
        }
        const _remainingPendingChanges = [];
        this.pendingChanges.forEach(c => {
            if (!_pendingChanges.some(d => idEquals(d.contentId, c.contentId) && d.contentTypeId === c.contentTypeId)) {
                _remainingPendingChanges.push(c);
            }
        })
        this.pendingChanges = _remainingPendingChanges;
        this.update();
        callBack && callBack();
    }

    mergeWithPendingChanges(contentId, contentTypeId, content) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const changesForContent = this.pendingChanges.find(c => idEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        const contentOriginal = {};
        Object.keys(content).forEach(k => {
            contentOriginal[`${k}_original`] = content[k] || null;
        })

        const contentMapping = { ...contentOriginal, ...content };
        if (!changesForContent) {
            return contentMapping;
        }

        changesForContent.changedFields.forEach(changedField => {
            contentMapping[changedField.name] = changedField.value;
        });

        return contentMapping;
    }
}

export default new ChangeTracker();