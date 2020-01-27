


/* COPY AS TAB SEPARATED */

class CopyAsTabSeparated {
    constructor(dataTable) {
        this.dataTable = dataTable;
    }

    copy() {
        return this.getPages()
            .then(pages => {
                var result = this.getHeaders();

                pages.forEach(page => page.Items.forEach(item => result += this.getRow(item)));

                return navigator.clipboard.writeText(result);
            });
    }

    getPages() {
        return this.dataTable.backend
            .load({
                page: 1,
                sortBy: this.dataTable.sortBy,
                sortDirection: this.dataTable.sortDirection,
            })
            .then(firstPage => {
                var pageNumbers = Array.from(Array(firstPage.PageCount).keys(), i => i + 1);

                pageNumbers.shift();

                var promises = pageNumbers.map(page =>
                        this.dataTable.backend.load({
                            page,
                            sortBy: this.dataTable.sortBy,
                            sortDirection: this.dataTable.sortDirection,
                        })
                    );

                return Promise.all(promises).then(pages => [firstPage].concat(pages));
            });
    }

    getHeaders() {
        var result = [];

        this.dataTable.columns
            .forEach(column => {
                if (column.shrink) {
                    return;
                }

                if (column.headerGenerator) {
                    var content = column.headerGenerator(this.dataTable);

                    if (content instanceof Node) {
                        result.push(content.innerText);
                    } else if (content.element instanceof Node) {
                        result.push(content.element.innerText);
                    } else {
                        result.push(content);
                    }
                }
            });

        return result.join('\t') + '\n';
    }

    getRow(item) {
        var result = [];

        this.dataTable.columns
            .forEach(column => {
                if (column.shrink) {
                    return;
                }

                if (column.contentGenerator) {
                    var content = column.contentGenerator(item, this.dataTable);

                    if (content instanceof Node) {
                        result.push(content.innerText);
                    } else if (content.element instanceof Node) {
                        result.push(content.element.innerText);
                    } else {
                        result.push(content);
                    }
                }
            });

        return result.join('\t') + '\n';
    }
}

export default CopyAsTabSeparated;