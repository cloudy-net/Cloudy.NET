import FieldControl from '../field-control.js';
import ListItem from '../../ListSupport/list-item.js';
import ItemProvider from './select-item-provider.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
//import ContextMenu from '../ContextMenuSupport/context-menu.js';
import List from '../../ListSupport/list.js';
import notificationManager from '../../NotificationSupport/notification-manager.js';



/* SELECT CONTROL */

class SelectControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var item = new ListItem();
        super(item.element);
        this.setupProvider(fieldModel, value, app, blade, item);
    }

    async setupProvider(fieldModel, value, app, blade, listItem) {
        var update = item => {
            listItem.setText(item.text);
        };

        if (typeof value == 'string') {
            update(await ItemProvider.get(fieldModel.descriptor.control.parameters['provider'], fieldModel.descriptor.control.parameters['type'], value));
        }

        listItem.setText('&nbsp;');

        var open = async () => {
            listItem.setActive(true);

            var list = new ListItemsBlade(app, fieldModel)
                .onSelect(item => {
                    this.triggerChange(item.value);
                    update(item);
                    app.close(list);
                })
                .onClose(() => listItem.setActive(false));

            app.openAfter(list, blade);
        };

        listItem.onClick(open);

        if (fieldModel.descriptor.isSortable && !fieldModel.descriptor.embeddedFormId) {
            open();
        }

        this.onSet(value => {
            update(value);
        });
    }
}



/* LIST ITEMS BLADE */

class ListItemsBlade extends Blade {
    onSelectCallbacks = [];

    constructor(app, fieldModel) {
        super();

        this.app = app;
        this.name = fieldModel.descriptor.label;
        this.provider = fieldModel.descriptor.control.parameters['provider'];
        this.type = fieldModel.descriptor.control.parameters['type'];
    }

    async open() {
        this.setTitle(`Select ${this.name.substr(0, 1).toLowerCase()}${this.name.substr(1)}`);

        //this.createNew = () => this.app.openAfter(new EditContentBlade(this.app, this.contentType).onComplete(() => update()), this);
        this.setToolbar(new Button('New').setInherit()/*.onClick(this.createNew)*/);

        var list = new List();
        this.setContent(list);

        var update = async () => {
            for (var item of await ItemProvider.getAll(this.provider, this.type)) {
                list.addItem(listItem => {
                    if (item.image) {
                        listItem.setImage(item.image);
                    }
                    listItem.setText(item.text);
                    listItem.onClick(() => {
                        listItem.setActive();
                        this.onSelectCallbacks.forEach(callback => callback.apply(this, [item]));
                    });

                });
            }
        };

        update();
    }

    onSelect(callback) {
        this.onSelectCallbacks.push(callback);

        return this;
    }
}

export default SelectControl;