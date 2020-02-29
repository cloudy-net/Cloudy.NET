import Sortable from '../sortable.js';
import SortableItem from '../sortable-item.js';
import FormBuilder from '../form-builder.js';
import DataTable from '../../DataTableSupport/data-table.js';
import Backend from '../../DataTableSupport/backend.js';
import Blade from '../../blade.js';
import Button from '../../button.js';
import ContextMenu from '../../ContextMenuSupport/context-menu.js';

class SortableTableControl extends Sortable {
    constructor(fieldModel, target, app) {
        super(fieldModel, target, index => {
            if (!(index in target)) {
                target[index] = {};
            }

            var container = document.createElement('tr');

            return new SortableItem(container, {});
        });

        this.fieldModel = fieldModel;

        var pageSize = target.length || 10;

        var backend = new class extends Backend {
            load(query) {
                return Promise.resolve({
                    items: target.slice((query.page - 1) * pageSize, query.page * pageSize),
                    pageCount: Math.ceil(target.length / pageSize),
                    pageSize: pageSize,
                    totalMatching: target.length,
                });
            }
        };

        var formBuilder = new FormBuilder(fieldModel.descriptor.embeddedFormId, app);

        var dataTable = new DataTable()
            .setBackend(backend);

        if (fieldModel.descriptor.control.parameters.columns) {
            for (const [name, column] of Object.entries(fieldModel.descriptor.control.parameters.columns)) {
                dataTable.addColumn(c => c
                    .setHeader(() => name)
                    .setContent(item => {
                        if (column.type != 'Expression') {
                            return column.value;
                        }

                        return column.value.segments.map(segment => {
                            if (segment.type == 'Interpolated') {
                                return item[segment.value];
                            }

                            return segment.value;
                        }).join('');
                    })
                )
            }
        } else {
            for (const columnFieldModel of fieldModel.fields) {
                dataTable.addColumn(c => c
                    .setHeader(() => columnFieldModel.descriptor.label)
                    .setContent(item => item[columnFieldModel.descriptor.camelCaseId])
                )
            }
        }

        dataTable
            .addColumn(c => c
                .setShrink()
                .setButtonColumn()
                .setContent(item =>
                    new ContextMenu()
                        .addItem(menuItem => menuItem.setText('Edit').onClick(() => app.open(new EditRow(formBuilder, item, app).onClose(message => { if (message == 'saved') { dataTable.update(); } }), dataTable.element)))
                        .addItem(menuItem => menuItem.setText('Remove').onClick(() => { target.splice(target.indexOf(item), 1); dataTable.update(); }))
                )
        )
            .setFooter(new Button('Add').setInherit().onClick(() => app.open(new EditRow(formBuilder, null, app).onClose((message, values) => { if (message == 'saved') { target.push(values); dataTable.update(); } }), dataTable.element)));

        dataTable.paging.remove();

        var container = document.createElement('cloudy-ui-form-table');
        container.append(dataTable.element);
        this.element = container;
    }
}

export default SortableTableControl;



/* EDIT ROW */

class EditRow extends Blade {
    constructor(formBuilder, item, app) {
        super();

        if (item) {
            this.setTitle('Edit');
        } else {
            this.setTitle('Add');
        }

        formBuilder.build(item).then(form => {
            this.setContent(form);
            this.setFooter(new Button('Ok').setPrimary().onClick(() => app.close(this, 'saved', form.getValues())), new Button('Cancel').onClick(() => app.close(this)));
        });

    }
}