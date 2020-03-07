


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
                items: [...data],
                pageCount: 1,
                pageSize: data.length,
                totalMatching: data.length,
            });
        }

        var sort = query.sortBy ? `&sortby=${query.sortBy}&sortdirection=${query.sortDirection}` : '';

        return fetch(`Backend/GetAll?provider=${this.name}&page=${query.page}${sort}`, { credentials: 'include' })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        if (text) {
                            throw new Error(text.split('\n')[0]);
                        } else {
                            text = response.statusText;
                        }

                        throw new Error(`${response.status} (${text})`);
                    });
                }

                return response.json();
            })
            .catch(error => notificationManager.addNotification(item => item.setText(`Could not get data from datatable backend ${this.name} (${error.message})`)));;
    }
}

export default Backend;