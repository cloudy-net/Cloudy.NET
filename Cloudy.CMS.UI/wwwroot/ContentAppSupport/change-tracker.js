import contentSaver from "./utils/content-saver.js";
import idEquals from "./utils/id-equals.js";

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

    save(contentId, contentTypeId, changedField) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const { name, value, originalValue } = changedField;
        let pendingChange = this.pendingChanges.find(c => idEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!pendingChange) {
            pendingChange = {
                contentId,
                contentTypeId,
                changedFields: []
            };

            this.pendingChanges.push(pendingChange);
        }

        const changeFieldIndex = pendingChange.changedFields.findIndex(f => f.name === name);
        if (changeFieldIndex === -1 && value !== originalValue) {
            pendingChange.changedFields.push(changedField);
            this.persistPendingChanges();
            this.triggerUpdate();
            return;
        }

        if (changeFieldIndex !== -1) {
            if ( value === originalValue) {
                pendingChange.changedFields.splice(changeFieldIndex, 1);
            } else {
                pendingChange.changedFields[changeFieldIndex].value = value;
            }
        }
        this.persistPendingChanges();
        this.triggerUpdate();
    }

    getFor(contentId, contentTypeId) {
        return this.pendingChanges.find(p => idEquals(p.contentId, contentId) && p.contentTypeId == contentTypeId);
    }

    reset(contentId, contentTypeId) {
        const _pendingChanges = this.pendingChanges;
        const index = _pendingChanges.findIndex(c => idEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);
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
        this.persistPendingChanges();
        this.triggerUpdate();
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