import urlFetcher from '../../url-fetcher.js';

/* CONTENT TYPE GROUP PROVIDER */

class ContentTypeGroupProvider {
    all;

    async getAll() {
        if (!this.all) {
            this.all = urlFetcher.fetch('ContentTypeGroupProvider/GetAll', { credentials: 'include' }, 'Could not get content type groups');
        }
        return await this.all;
    }
}

export default new ContentTypeGroupProvider();