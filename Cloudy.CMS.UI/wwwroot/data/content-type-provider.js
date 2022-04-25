import urlFetcher from '../util/url-fetcher.js';

class ContentTypeProvider {
    _allPromise;
    _all;

    async getAll() {
        if (!this._allPromise) {
            this._allPromise = urlFetcher.fetch('ContentTypeProvider/GetAll', { credentials: 'include' }, 'Could not get content types');
        }

        this._all = await this._allPromise;

        return this._all;
    }

    get(id) {
        if (!this._all) {
            return null;
        }

        return this._all.find(contentType => contentType.id == id);
    }
}

export default new ContentTypeProvider();