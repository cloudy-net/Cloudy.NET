import urlFetcher from '../util/url-fetcher.js';

class ContentTypeProvider {
    _all;

    async load() {
        this._all = await urlFetcher.fetch('ContentTypeProvider/GetAll', { credentials: 'include' }, 'Could not get content types');
    }

    getAll() {
        return this._all;
    }

    get(id) {
        return this._all.find(contentType => contentType.id == id);
    }
}

export default new ContentTypeProvider();