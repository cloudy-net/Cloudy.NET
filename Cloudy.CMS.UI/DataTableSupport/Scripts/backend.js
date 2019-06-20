


/* BACKEND */

class Backend {
    constructor(arg) {
        if (typeof arg == 'string') {
            this.name = arg;
        }
        if (arg instanceof Array) {
            this.data = arg;
        }
    }

    load(query) {
        if (this.data) {
            var data = [...this.data];

            var direction = query.sortDirection == 'ascending' ? 1 : -1;

            if (query.sortBy) {
                data.sort((a, b) => {
                    a = a[query.sortBy];
                    b = b[query.sortBy];

                    if (!a) {
                        return -1 * direction;
                    }

                    if (!b) {
                        return 1 * direction;
                    }

                    if (typeof a == 'string' && typeof b == 'string') {
                        return a.localeCompare(b, 'en', { sensitivity: 'base' }) * direction;
                    }

                    return (a > b) * direction;
                });
            }

            return Promise.resolve({
                Items: [...data],
                PageCount: 1,
                PageSize: data.length,
                TotalMatching: data.length,
            });
        }

        var sort = query.sortBy ? `&sortby=${query.sortBy}&sortdirection=${query.sortDirection}` : '';

        return fetch(`Poetry.UI.DataTableSupport/Backend/GetAll?provider=${this.name}&page=${query.page}${sort}`, { credentials: 'include' })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`DataTable backend returned ${response.status} (${response.statusText})`);
                }

                return response.json();
            });
    }
}

export default Backend;