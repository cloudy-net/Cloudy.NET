import urlFetcher from "../util/url-fetcher.js";

class ContentTypeProvider {
    all = [];
    allById = {};
    get(contentTypeId){
        return this.allById[contentTypeId];
    }
    async fetch(){
        this.all = await urlFetcher.fetch('ContentTypeProvider/GetAll', { credentials: 'include' }, 'Could not get content types')
        this.all.forEach(contentType => this.allById[contentType.id] = contentType);
    }
};

export default new ContentTypeProvider();