import urlFetcher from '../../util/url-fetcher.js';

class FieldDescriptorProvider {
    allById = {};
    get(contentTypeId) {
        return this.allById[contentTypeId];
    }
    async fetch(){
        this.allById = await urlFetcher.fetch(`Field/GetAll`, { credentials: 'include' }, `Could not get field descriptors`);
    }
}

export default new FieldDescriptorProvider();
