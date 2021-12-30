import html from '../util/html.js';
import PendingChangesContext from './pending-changes-context.js';
import { useState, useCallback, useEffect } from '../lib/preact.hooks.module.js';
const _pendingChangesKey = '_pendingChanges';

function PendingChangesContextProvider({ children }) {
    const [pendingChanges, setPendingChanges] = useState(JSON.parse(localStorage.getItem(_pendingChangesKey)) || []);

    // update pendingChanges
    useEffect(() => {
        localStorage.setItem(_pendingChangesKey, JSON.stringify(pendingChanges));
    }, [pendingChanges]);

    const arrayEquals = (a, b) => {
        if (a == null && b == null) {
            return true;
        }

        if (a == null || b == null) {
            return false;
        }

        if (Array.isArray(a) && Array.isArray(b)) {
            return a.every((ai, i) => ai === b[i]);
        }

        return a === b;
    }

    const resetChange = useCallback((contentId, contentTypeId) => {
        const _pendingChanges = [...pendingChanges];
        const index = _pendingChanges.findIndex(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);
        if (index !== -1) {
            _pendingChanges.splice(index, 1);
            setPendingChanges(pendingChanges)
        }
    }, [pendingChanges])

    const updatePendingChanges = useCallback((changes) => {
        const { keys, contentTypeId, change } = changes;
        const contentId = (keys && keys[0]) || null;

        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const _pendingChanges = [...pendingChanges];
        let changesForContent = _pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!changesForContent) {
            changesForContent = { contentId, contentTypeId, changedFields: [] };
            _pendingChanges.push(changesForContent);
        }

        if (change.remove === true) {
            changesForContent.remove = true;
        }
        if (change.remove === false) {
            delete changesForContent.remove;
        }

        let changedField = changesForContent.changedFields.find(f => arrayEquals(change.path, f.path));

        if (change.type == 'simple') {
            if (!changedField) {
                changesForContent.changedFields.push(changedField = change);
            }

            if (change.operation == 'set') {
                changedField.value = change.value;
            }
        }

        if (change.type == 'array') {
            if (!changedField) {
                changesForContent.changedFields.push(changedField = { path: change.path, type: 'array', changes: [] });
            }

            if (change.operation == 'add') {
                changedField.changes.push({ id: change.id, type: change.type, value: JSON.stringify(change.value) });
            }
            if (change.operation == 'update') {
                const item = changedField.changes.find(i => i.id == change.id);
                item.value = change.value;
            }
            if (change.operation == 'delete') {
                var item = changedField.changes.find(i => i.id == change.id);
                if (item.operation == 'add') {
                    changedField.changes.splice(changedField.changes.indexOf(item), 1); // delete addition completely
                    if (!changedField.changes.length) {
                        changesForContent.changedFields = [];
                    }
                } else {
                    item.operation = 'delete';
                    changedField.changes.splice(changedField.changes.indexOf(item), 1);
                }
            }
        }

        if (changesForContent.changedFields.length == 0 && !changesForContent.remove) {
            _pendingChanges.splice(_pendingChanges.indexOf(changesForContent), 1); // delete empty change object
        }

        setPendingChanges(_pendingChanges);
    }, [pendingChanges]);

    const getPendingValue = useCallback((contentId, contentTypeId, path, value) => {
        if (!contentId && contentId !== null) {
            throw new Error('ContentId must be null or a valid value (string, number, ...)')
        }

        const changesForContent = pendingChanges.find(c => arrayEquals(contentId, c.contentId) && c.contentTypeId === contentTypeId);

        if (!changesForContent) {
            return value;
        }

        const changedField = changesForContent.changedFields.find(c => arrayEquals(path, c.path));

        if (!changedField) {
            return value;
        }

        return changedField.value;
    }, [pendingChanges]);

    const getFor = useCallback((contentId, contentTypeId) => {
        return pendingChanges.find(p => arrayEquals(p.contentId, contentId) && p.contentTypeId == contentTypeId);
    }, []);

    return html`
        <${PendingChangesContext.Provider} value=${[pendingChanges, updatePendingChanges, resetChange, getPendingValue, getFor]}>
            ${children}
        <//>
    `;
}

export default PendingChangesContextProvider;
