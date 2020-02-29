import Button from '../button.js';
import Backend from './backend.js';



/* DATA TABLE */

class DataTable {
    constructor() {
        this.columns = [];
        this.page = 1;

        this.element = document.createElement('cloudy-ui-data-table');

        this.header = document.createElement('cloudy-ui-data-table-header');
        this.header.style.display = 'none';
        this.element.appendChild(this.header);

        this.tableOuter = document.createElement('div');
        this.tableOuter.classList.add('cloudy-ui-data-table-outer');
        this.element.appendChild(this.tableOuter);

        var table = document.createElement('table');
        table.classList.add('cloudy-ui-data-table');
        this.tableOuter.appendChild(table);

        var tableHeader = document.createElement('thead');
        table.appendChild(tableHeader);

        this.columnHeaderRow = document.createElement('tr');
        tableHeader.appendChild(this.columnHeaderRow);

        this.tableBody = document.createElement('tbody');
        table.appendChild(this.tableBody);

        this.footer = document.createElement('cloudy-ui-data-table-footer');
        this.element.appendChild(this.footer);

        this.paging = document.createElement('cloudy-ui-data-table-paging');
        this.footer.appendChild(this.paging);
    }

    setBackend(arg) {
        this.backend = arg instanceof Backend ? arg : new Backend(arg);

        this.update();

        return this;
    }

    addColumn(callback) {
        var column = new Column();

        callback(column);

        this.columns.push(column);

        return this;
    }

    update() {
        this.element.classList.add('cloudy-ui-loading');
        this.element.classList.remove('cloudy-ui-not-loading');

        this.backend.load({
            page: this.page,
            sortBy: this.sortBy,
            sortDirection: this.sortDirection,
        })
            .then(response => {
                this.element.classList.remove('cloudy-ui-loading');
                this.element.classList.add('cloudy-ui-not-loading');

                if (!this.columnHeaderRow.children.length) {
                    var shrinkColumnCount = this.columns.filter(a => a.shrink).length;

                    this.columns.forEach(column => {
                        var element = document.createElement('th');
                        element.style.width = column.shrink ? '1%' : (Math.floor((100 - shrinkColumnCount) / (this.columns.length - shrinkColumnCount) * 100) / 100) + '%';

                        if (column.headerGenerator) {
                            var result = column.headerGenerator(this);

                            if (result) {
                                element.append(result.element || result);
                            }
                        }

                        if (column.sorting) {
                            var sorter = document.createElement('cloudy-ui-data-table-sorter');

                            sorter.tabIndex = 0;
                            sorter.addEventListener('click', () => {
                                [...this.columnHeaderRow.querySelectorAll('cloudy-ui-data-table-sorter')].forEach(e => e.classList.remove('cloudy-ui-active', 'cloudy-ui-descending'));

                                if (this.sortBy == column.sorting && this.sortDirection == 'descending') {
                                    this.sortBy = null;
                                    this.sortDirection = null;

                                    this.update();

                                    return;
                                }

                                sorter.classList.add('cloudy-ui-active');

                                if (this.sortBy != column.sorting) {
                                    this.sortBy = column.sorting;
                                    this.sortDirection = 'ascending';

                                    sorter.classList.remove('cloudy-ui-descending');

                                    this.update();

                                    return;
                                }

                                this.sortDirection = this.sortDirection == 'ascending' ? 'descending' : 'ascending';

                                if (this.sortDirection == 'descending') {
                                    sorter.classList.add('cloudy-ui-descending');
                                }

                                this.update();
                            });
                            sorter.addEventListener("keyup", event => {
                                if (event.keyCode != 13) {
                                    return;
                                }

                                event.preventDefault();
                                sorter.click();
                            });

                            element.appendChild(sorter);
                        }

                        this.columnHeaderRow.appendChild(element);
                    });
                }

                this.tableOuter.style.minHeight = this.tableOuter.offsetHeight ? Math.max(parseInt(this.tableOuter.offsetHeight), this.tableOuter.offsetHeight) + 'px' : this.tableOuter.offsetHeight + 'px';

                [...this.tableBody.children].forEach(c => this.tableBody.removeChild(c));
                
                response.items.forEach(item => {
                    var row = document.createElement('tr');
                    this.tableBody.appendChild(row);

                    this.columns.forEach(column => {
                        var element = document.createElement('td');

                        if (column.buttonColumn) {
                            element.classList.add('cloudy-ui-data-table-button-column');
                        }

                        if (column.contentGenerator) {
                            var result = column.contentGenerator(item, this);

                            if (result) {
                                element.append(result.element || result);
                            }
                        }

                        row.appendChild(element);
                    });
                });

                if (response.pageCount > 1) {
                    this.paging.style.display = '';
                } else {
                    this.paging.style.display = 'none';
                }

                [...this.paging.children].forEach(c => this.paging.removeChild(c));

                var pages = Array(response.pageCount).fill().map((a, i) => i + 1);
                var pagination = [];

                var startIndex = 0;

                if (this.page > 3) {
                    pagination.push(pages[0]);
                    pagination.push('...');
                    startIndex = this.page - 2;
                    startIndex = Math.max(startIndex, 2);
                    startIndex = Math.min(startIndex, pages.length - 4);
                }

                var stopIndex = Math.min(startIndex + 3, pages.length - 1);

                for (var i = startIndex; i <= stopIndex; i++) {
                    pagination.push(pages[i]);
                }

                if (stopIndex < pages.length - 3) {
                    pagination.push('...');
                    pagination.push(pages[pages.length - 1]);
                } else {

                    for (var i = stopIndex + 1; i < pages.length; i++) {
                        pagination.push(pages[i]);
                    }
                }

                new Button('')
                    .onClick(() => {
                        if (this.page == 1) {
                            return;
                        }

                        this.page = this.page - 1;
                        this.update();
                    })
                    .addClass('cloudy-ui-data-table-paging-previous')
                    .setDisabled(this.page == 1)
                    .appendTo(this.paging);

                pagination.forEach(page => {
                    var button = new Button(page)
                        .onClick(() => {
                            if (page == '...') {
                                return;
                            }

                            this.page = page;
                            this.update();
                        })
                        .addClass('cloudy-ui-active', this.page == page)
                        .appendTo(this.paging);

                    if (this.page == page) {
                        button.element.focus();
                    }
                });

                new Button('')
                    .onClick(() => {
                        if (this.page == response.pageCount) {
                            return;
                        }

                        this.page = this.page + 1;
                        this.update();
                    })
                    .addClass('cloudy-ui-data-table-paging-next')
                    .setDisabled(this.page == response.pageCount)
                    .appendTo(this.paging);
            });
    }

    setHeader(...items) {
        this.header.style.display = '';
        [...this.header.children].forEach(c => this.header.removeChild(c));
        items.forEach(item => this.header.append(item.element || item));

        return this;
    }

    setFooter(...items) {
        this.footer.style.display = '';
        [...this.footer.children].forEach(c => this.footer.removeChild(c));
        items.forEach(item => this.footer.append(item.element || item));

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}



/* COLUMN */

class Column {
    setShrink() {
        this.shrink = true;

        return this;
    }

    setButtonColumn(value = true) {
        this.buttonColumn = value;

        return this;
    }

    setHeader(headerGenerator) {
        this.headerGenerator = headerGenerator;

        return this;
    }

    setContent(contentGenerator) {
        this.contentGenerator = contentGenerator;

        return this;
    }

    setSorting(name) {
        this.sorting = name;

        return this;
    }
}

export default DataTable;