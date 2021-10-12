import urlFetcher from "../url-fetcher.js";

/* FORM FIELD DESCRIPTOR PROVIDER */

class FormFieldDescriptorProvider {
    promises = {};

    async getFor(formId) {
        if (!this.promises[formId]) {
            this.promises[formId] = urlFetcher.fetch(`Field/GetAllForForm?id=${formId}`, { credentials: 'include' }, `Could not get field descriptors for form \`${formId}\``);
        }
        return await this.promises[formId];
    }
}

export default new FormFieldDescriptorProvider();