import Button from "../button.js";
import contentSaver from "./content-saver.js";
import PendingChangesBlade from "./pending-changes-blade.js";

/* CHANGE TRACKER */

class ChangeTracker {
    element = document.createElement('cloudy-ui-change-tracker');
    _pendingChanges;
    _pendingChangesKey = '_pendingChangesKey';
    changeExecutors = {
        save: contentSaver
    };
    referenceEvents = [];

    constructor(app, parentBlade) {
        this.button = new Button('No changes').setDisabled().appendTo(this.element);
        this.app = app;
        this.parentBlade = parentBlade;
        this.button.onClick(() => this.saveChange());
        this.setReferenceEvents(this.button);
        this.update();
    }
    
    setReferenceEvents(element, type = 'primary') {
        const index = this.referenceEvents.findIndex(e => e.target.id === element.id);
        if (index !== -1) {
            this.referenceEvents.splice(index, 1);
        }
        this.referenceEvents.push({ target: element, type });
    }

    saveChange() {
        this.app.addBladeAfter(new PendingChangesBlade(this.app, this), this.parentBlade);
    }

    save(contentId, contentTypeId, contentAsJson) {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const { name, value, originalValue } = contentAsJson;
        const _pendingChangeToSave = this.pendingChanges;
        const index = _pendingChangeToSave.findIndex(c => c.type === 'save' && (contentId === null || c.contentId.every(function (id, i) { return id === contentId[i] })) && c.contentTypeId === contentTypeId);
        if (index === -1) {
            _pendingChangeToSave.push({
                type: 'save',
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

    update(pendingChanges = this.pendingChanges) {
        let changeCount = 0;
        pendingChanges.forEach(c => {
            changeCount = changeCount + c.changedFields.length;
        });
        const changeText = changeCount <= 0 ? 'No changes': (changeCount > 1 ? `${changeCount} changes` : '1 change');
        this.referenceEvents.forEach(element => {
            if (element.type === 'primary') {
                element.target.setText(changeCount <= 0 ? (element.target.initText || changeText) : changeText);
                element.target.setPrimary(changeCount > 0);
            }
            element.target.setDisabled(changeCount <= 0);
        });
        this.pendingChanges = pendingChanges;
    }

    reset(contentId, contentTypeId) {
        const _pendingChanges = this.pendingChanges;
        const index = _pendingChanges.findIndex(c => (contentId === null || c.contentId.every(function (id, i) { return id === contentId[i] })) && c.contentTypeId === contentTypeId);
        if (index !== -1) {
            _pendingChanges.splice(index, 1);
            this.update(_pendingChanges);
        }
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
            if (!_pendingChanges.some(d => (d.contentId === null || c.contentId.every(function (id, i) { return id === d.contentId[i] })) && d.contentTypeId === c.contentTypeId)) {
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

        const changesForContent = this.pendingChanges.find(c => (contentId === null || c.contentId.every(function (id, i) { return id === contentId[i] })) && c.contentTypeId === contentTypeId);

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

    set pendingChanges(value) {
        this._pendingChanges = value;
        localStorage.setItem(this._pendingChangesKey, JSON.stringify(value));
    }

    get pendingChanges() {
        return this._pendingChanges || JSON.parse(localStorage.getItem(this._pendingChangesKey)) || [];
    }
}

export default ChangeTracker;