import Blade from "../blade.js";
import Button from "../button.js";
import Diff from './lib/diff.js'
import fieldDescriptorProvider from '../FormSupport/field-descriptor-provider.js';
import contentTypeProvider from "../list-content-types/content-type-provider.js";
import contentGetter from "./utils/content-getter.js";
import EditContentBlade from './edit-content-blade.js';
import changeTracker from "./utils/change-tracker.js";

/* PENDING CHANGES DIFF BLADE */

class PendingChangesDiffBlade extends Blade {
    undoCallbacks = [];

    constructor(app, change, contentType) {
        super();
        this.app = app;
        this.change = change;
        this.contentType = contentType;
        this.setTitle('Pending changes' + (change.changedFields.length ? ` (${change.changedFields.length})` : ''));
        this.setToolbar(new Button('Edit').setInherit().onClick(async () => {
            const content = await contentGetter.get(this.change.contentId, this.change.contentTypeId);
            const blade = new EditContentBlade(this.app, this.contentType, content);
            await this.app.addBladeAfter(blade, this.app.listContentTypesBlade);
        }));
        this.undoChangesButton = new Button('Undo changes')
            .setStyle({ marginLeft: 'auto' })
            .onClick(async () => {
                if (confirm('Undo changes? This is not reversible')) {
                    changeTracker.reset(this.change.contentId, this.change.contentTypeId);
                    await this.app.removeBlade(this);
                    this.undoCallbacks.forEach(callback => callback());
                }
            });
        this.saveButton = new Button('Save')
            .setPrimary()
            .setStyle({ marginLeft: '10px' })
            .onClick(async () => {
                changeTracker.apply([this.change], () => {
                    changeTracker.reset(this.change.contentId, this.change.contentTypeId);
                })
            });
        this.setFooter(
            this.undoChangesButton,
            this.saveButton,
        );
    }

    async open() {
        if (this.change.remove) {
            var div = document.createElement('div');
            div.innerText = `${this.contentType.name} will be deleted`;
            div.style.padding = '16px';
            this.setContent(div);
            return;
        }

        var form = document.createElement('div');
        form.classList.add('cloudy-ui-form');
        var fields = await fieldDescriptorProvider.getFor(this.change.contentTypeId);

        for (const changedField of this.change.changedFields) {
            var element = document.createElement('div');
            element.classList.add('cloudy-ui-form-field');

            const name = changedField.path[changedField.path.length - 1];
            var field = fields.find(f => f.id == name);

            var heading = document.createElement('div');
            heading.classList.add('cloudy-ui-form-field-label');
            heading.innerText = field.label;
            element.appendChild(heading);

            var input = document.createElement('div');
            input.classList.add('cloudy-ui-form-input');

            var textarea = document.createElement('textarea');
            var value = '';
            for (const [state, segment] of Diff(changedField.initialValue || '', changedField.value, 0)) {
                textarea.innerHTML = segment

                switch (state) {
                    case Diff.DELETE: value += `<span class="" style="background-color: #ffebe9; padding: 0 1px;">${textarea.innerHTML}</span>`; break;
                    case Diff.EQUAL: value += `<span>${textarea.innerHTML}</span>`; break;
                    case Diff.INSERT: value += `<span class="cloudy-ui-diff-insert" style="background-color: #e6ffec; padding: 0 1px;">${textarea.innerHTML}</span>`; break;
                }
            }
            input.innerHTML = value;
            
            element.append(input);

            form.append(element);
        }

        this.setContent(form);
    }

    onUndo(callback) {
        this.undoCallbacks.push(callback);
        return this;
    }
}

export default PendingChangesDiffBlade;