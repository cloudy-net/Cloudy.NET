import FieldControl from '../field-control.js';
import ListContentBlade from '../../ContentAppSupport/list-content-blade.js';
import ListItem from '../../ListSupport/list-item.js';
import ContentTypeProvider from '../../DataSupport/content-type-provider.js';
import ContentGetter from '../../DataSupport/content-getter.js';

class SelectControl extends FieldControl {
    constructor(fieldModel, value, app, blade) {
        var item = new ListItem();
        super(item.element);

        var getContentType = ContentTypeProvider.get(fieldModel.descriptor.control.parameters['contenttype']);

        var updateId = async id => {
            if (!id) {
                item.setText('(nothing selected)');
                return;
            }

            var contentType = await getContentType;
            update(await ContentGetter.get(id, contentType.id));
        };

        var update = async content => {
            var contentType = await getContentType;
            var name;

            if (contentType.isNameable) {
                name = contentType.nameablePropertyName ? content[contentType.nameablePropertyName] : content.name;

                if (!name) {
                    name = `${contentType.name} ${content.id}`;
                }
            } else {
                name = content.id;
            }

            item.setText(name);
        };

        item.setText('&nbsp;');

        if (typeof value == 'string') {
            var timeout = setTimeout(() => item.setText('(loading name...)'), 200);
            updateId(value).then(() => clearTimeout(timeout));
        }

        var open = async () => {
            item.setActive(true);

            var contentType = await getContentType;

            var list = new ListContentBlade(app, contentType)
                .onSelect(content => {
                    this.triggerChange(content.id);
                    update(content);
                    app.close(list);
                })
                .onClose(() => item.setActive(false));

            app.openAfter(list, blade);
        };

        item.onClick(open);

        if (fieldModel.descriptor.isSortable && !fieldModel.descriptor.embeddedFormId) {
            open();
        }

        this.onSet(value => {
            update(value);
        });
    }
}

export default SelectControl;