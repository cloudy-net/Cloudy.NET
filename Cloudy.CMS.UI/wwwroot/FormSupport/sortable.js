import SortableMenu from './sortable-menu.js';

class SortableField {
    callbacks = {
        add: [],
        move: [],
        delete: [],
    };
    items = [];

    constructor() {
        this.fields = document.createElement('cloudy-ui-sortable-items');
        this.element = document.createElement('cloudy-ui-sortable');
        this.element.appendChild(this.fields);

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
            this.items.splice(newIndex, 0, this.items.splice(index, 1)[0]);
        });
    }

    setHorizontal() {
        this.horizontal = true;
        return this;
    }

    addFooter(element) {
        var buttonContainer = document.createElement('cloudy-ui-sortable-buttons');
        buttonContainer.appendChild(element.element || element);
        this.element.appendChild(buttonContainer);
    }

    add(item, triggerEvent = true) {
        this.items.push(item);

        var field = document.createElement('cloudy-ui-sortable-item');
        this.fields.appendChild(field);

        var fieldAction = document.createElement('cloudy-ui-sortable-item-action');
        field.appendChild(fieldAction);

        const menu = new SortableMenu(this, item).appendTo(fieldAction);
        
        if (this.horizontal) {
            menu.setHorizontal();
            field.append(fieldAction);
        } else {
            menu.setCompact();
            item.element.append(fieldAction);
        }

        field.appendChild(item.element);

        if (triggerEvent) {
            this.triggerAdd(item);
        }
    }

    triggerAdd(item) {
        this.callbacks.add.forEach(callback => callback(item));
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

    triggerDelete(index) {
        this.callbacks.delete.forEach(callback => callback(index));
    }

    onDelete(callback) {
        this.callbacks.delete.push(callback);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default SortableField;