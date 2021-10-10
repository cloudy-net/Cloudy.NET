import Blade from "../blade.js";
import Button from "../button.js";
import Diff from './lib/diff.js'



/* PENDING CHANGES DIFF BLADE */

class PendingChangesDiffBlade extends Blade {
    changeTracker;
    constructor(app, change) {
        super();
        this.app = app;
        this.change = change;

        this.setTitle(`Pending changes (${change.changedFields.length})`);
        this.saveButton = new Button('Undo changes');
        this.setFooter(
            this.saveButton
                .setStyle({ marginLeft: 'auto' })
                .onClick(async () => {
                    confirm('Undo changes? This is not reversible');
                })
        );
    }

    async open() {
        var form = document.createElement('div');
        form.classList.add('cloudy-ui-form');

        for (const changedField of this.change.changedFields) {
            var element = document.createElement('div');
            element.classList.add('cloudy-ui-form-field');

            var heading = document.createElement('div');
            heading.classList.add('cloudy-ui-form-field-label');
            heading.innerText = changedField.name;
            element.appendChild(heading);

            var input = document.createElement('div');
            input.classList.add('cloudy-ui-form-input');

            var value = '';
            for (const [state, segment] of Diff(changedField.originalValue, changedField.value, 0)) {
                switch (state) {
                    case Diff.DELETE: value += `<span class="cloudy-ui-diff-delete" style="background-color: #ffebe9; padding: 0 1px;">${segment}</span>`; break;
                    case Diff.EQUAL: value += `<span>${segment}</span>`; break;
                    case Diff.INSERT: value += `<span class="cloudy-ui-diff-insert" style="background-color: #e6ffec; padding: 0 1px;">${segment}</span>`; break;
                }
            }
            input.innerHTML = value;
                
            element.append(input);

            form.append(element);
        }

        this.setContent(form);
    }
}

export default PendingChangesDiffBlade;