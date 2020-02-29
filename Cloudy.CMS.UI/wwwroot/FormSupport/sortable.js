import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';

class SortableField {
    constructor(fieldModel, target, createItem) {
        this.callbacks = {
            add: [],
            move: [],
        };
        this.element = document.createElement('cloudy-ui-sortable');
        this.fields = document.createElement('cloudy-ui-sortable-items');
        this.element.appendChild(this.fields);
        this.items = [];

        this.target = target;
        this.createItem = createItem;

        target.forEach((_, index) => this.addItem(this.createItem(index)));

        this.onMove((index, newIndex) => {
            if (newIndex < index) {
                this.fields.insertBefore(this.fields.children[index], this.fields.children[newIndex]);
            } else {
                if (this.fields.children[newIndex + 1]) {
                    this.fields.insertBefore(this.fields.children[index], this.fields.children[newIndex + 1]);
                } else {
                    this.fields.appendChild(this.fields.children[index]);
                }
            }
            this.target.splice(newIndex, 0, this.target.splice(index, 1)[0]);
            this.items.splice(newIndex, 0, this.items.splice(index, 1)[0]);
        });

        var buttonContainer = document.createElement('cloudy-ui-sortable-buttons');
        this.element.appendChild(buttonContainer);

        new Button('Add')
            .setInherit()
            .onClick(() => {
                var item = this.createItem(this.target.length);
                this.addItem(item);
                this.triggerAdd(item);
            })
            .appendTo(buttonContainer);
    }

    addItem(item) {
        this.items.push(item);

        var field = document.createElement('cloudy-ui-sortable-item');
        this.fields.appendChild(field);

        field.appendChild(item.element);

        var fieldAction = document.createElement('cloudy-ui-sortable-item-action');
        (item.data.actionContainer || field).appendChild(fieldAction);

        new ContextMenu()
            .addItem(item => {
                item
                    .setText('Move up')
                    .onClick(() => {
                        var index = [...this.fields.children].indexOf(field);

                        if (index == 0) {
                            return;
                        }

                        var newIndex = index - 1;
                        
                        this.triggerMove(index, newIndex);
                    });
            })
            .addItem(item => {
                item
                    .setText('Move down')
                    .onClick(() => {
                        var index = [...this.fields.children].indexOf(field);

                        if (index == this.fields.children.length - 1) {
                            return;
                        }

                        var newIndex = index + 1;

                        this.triggerMove(index, newIndex);
                    });
            })
            .addItem(item => item.setText('Delete'))
            .appendTo(fieldAction);
    }

    triggerAdd(value) {
        this.callbacks.add.forEach(callback => callback(value));
    }

    onAdd(callback) {
        this.callbacks.add.push(callback);

        return this;
    }

    triggerMove(index, newIndex) {
        this.callbacks.move.forEach(callback => callback(index, newIndex));
    }

    onMove(callback) {
        this.callbacks.move.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default SortableField;