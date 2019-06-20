import Sortable from '../../../Scripts/sortable.js';
import SortableItem from '../../../Scripts/sortable-item.js';
import FormBuilder from '../../../Scripts/form-builder.js';
import DataTable from '../../../../Poetry.UI.DataTableSupport/Scripts/data-table.js';
import Backend from '../../../../Poetry.UI.DataTableSupport/Scripts/backend.js';
import Blade from '../../../../Poetry.UI/Scripts/blade.js';
import Button from '../../../../Poetry.UI/Scripts/button.js';
import ContextMenu from '../../../../Poetry.UI.ContextMenuSupport/Scripts/context-menu.js';

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

        var pageSize = target.length;

        var backend = new class extends Backend {
            load(query) {
                return Promise.resolve({
                    Items: target.slice((query.page - 1) * pageSize, query.page * pageSize),
                    PageCount: Math.ceil(target.length / pageSize),
                    PageSize: pageSize,
                    TotalMatching: target.length,
                });
            }
        };

        var formBuilder = new FormBuilder(fieldModel.descriptor.EmbeddedFormId, app);

        var dataTable = new DataTable()
            .setBackend(backend);

        if (fieldModel.descriptor.Control.Parameters.columns) {
            for (const [name, column] of Object.entries(fieldModel.descriptor.Control.Parameters.columns)) {
                dataTable.addColumn(c => c
                    .setHeader(() => name)
                    .setContent(item => {
                        if (column.Type != 'Expression') {
                            return column.Value;
                        }

                        return column.Value.Segments.map(segment => {
                            if (segment.Type == 'Interpolated') {
                                return item[segment.Value];
                            }

                            return segment.Value;
                        }).join('');
                    })
                )
            }
        }

        dataTable
            .addColumn(c => c
                .setShrink()
                .setContent(item =>
                    new ContextMenu()
                        .addItem(menuItem => menuItem.setText('Edit').onClick(() => app.openBlade(new EditRow(formBuilder, item).onClose(message => { if (message == 'saved') { dataTable.update(); } }), dataTable.element)))
                        .addItem(menuItem => menuItem.setText('Remove').onClick(() => { target.splice(target.indexOf(item), 1); dataTable.update(); }))
                )
            )
            .setFooter(new Button('Add').onClick(() => app.openBlade(new NewRow(formBuilder).onClose((message, values) => { if (message == 'saved') { target.push(values); dataTable.update(); } }), dataTable.element)));

        dataTable.paging.remove();

        this.element = dataTable.element;
    }
}

export default SortableTableControl;



/* EDIT ROW */

class EditRow extends Blade {
    constructor(formBuilder, item) {
        super();

        this.setTitle('Edit');

        formBuilder.build(item).then(form => {
            this.setContent(form);
            this.setFooter(new Button('Ok').onClick(() => this.close('saved', form.getValues())), new Button('Cancel').onClick(() => this.close()));
        });

    }
}



/* NEW ROW */

class NewRow extends Blade {
    constructor(formBuilder) {
        super();

        this.setTitle('Add');

        formBuilder.build().then(form => {
            this.setContent(form);
            this.setFooter(new Button('Ok').onClick(() => this.close('saved', form.getValues())), new Button('Cancel').onClick(() => this.close()));
        });

    }
}