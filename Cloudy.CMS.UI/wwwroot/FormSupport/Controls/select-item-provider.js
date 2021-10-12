import urlFetcher from "../../url-fetcher.js";

class SelectItemProvider {
    async getCreationForm(provider) {
        return await urlFetcher.fetch(`SelectControl/GetCreationForm?provider=${provider}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        }, `Could not get creation action for select control ${provider}`);
    }

    async get(provider, type, value) {
        return await urlFetcher.fetch(`SelectControl/GetItem?provider=${provider}&type=${type}&value=${value}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        }, `Could not get item \`${value}\` of type \`${type}\` for select control \`${provider}\``);
    }

    async getAll(provider, type, query) {
        var url = `SelectControl/GetItems?provider=${provider}&type=${type}`;

        if (query.parent) {
            url += `&parent=${query.parent}`;
        }

        return await urlFetcher.fetch(url, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        }, `Could not get items of type \`${type}\` for select control \`${provider}\``);
    }

    async getProvider(id) {
        return await urlFetcher.fetch(`SelectControl/GetProvider?id=${id}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        }, `Could not get item \`${value}\` for select control \`${provider}\``);
    }
}

export default new SelectItemProvider();