import urlFetcher from '../url-fetcher.js';

/* CONTENT TYPE PROVIDER */

class ContentTypeProvider {
    all;

    async getAll() {
        if (!this.all) {
            this.all = urlFetcher.fetch('ContentTypeProvider/GetAll', { credentials: 'include' }, 'Could not get content types');
        }

        return await this.all;
    }

    async get(id) {
        var contentTypes = await this.getAll();

        return contentTypes.find(contentType => contentType.id == id);
    }
}

export default new ContentTypeProvider();