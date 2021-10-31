import Button from '../button.js';
import ContextMenu from '../ContextMenuSupport/context-menu.js';

class SortableMenu extends ContextMenu {
    constructor(sortable, item) {
        super();
        this.addItem(item => item.setText('Move up').onClick(() => {
            var index = [...this.fields.children].indexOf(field);

            if (index == 0) {
                return;
            }

            var newIndex = index - 1;

            this.triggerMove(index, newIndex);
        }));
        this.addItem(item => item.setText('Move down').onClick(() => {
            var index = [...this.fields.children].indexOf(field);

            if (index == this.fields.children.length - 1) {
                return;
            }

            var newIndex = index + 1;

            this.triggerMove(index, newIndex);
        }));
        this.addItem(item => item.setText('Delete').onClick(() => {
            var index = [...this.fields.children].indexOf(field);
            this.fields.children[index].remove();
            this.items.splice(index, 1);
            this.triggerDelete(index);
        }));
    }

    appendTo(element) {
        element.appendChild(this.popup.element);

        return this;
    }
}

export default SortableMenu;