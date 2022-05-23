import urlFetcher from '../../util/url-fetcher.js';
import fieldControlProvider from './field-control-provider.js';

class FieldModelBuilder {
    async getFieldModels() {
        const fieldDescriptorsByContentTypeId = await urlFetcher.fetch(`Field/GetAll`, { credentials: 'include' }, `Could not get field descriptors`);
        const result = {};
        
        for(let contentTypeId of Object.keys(fieldDescriptorsByContentTypeId)){
            var fieldModelPromises = fieldDescriptorsByContentTypeId[contentTypeId]
                .map(fieldDescriptor => this.getFieldModel(fieldDescriptor));
    
            result[contentTypeId] = await Promise.all(fieldModelPromises);
        }

        return result;
    }

    async getFieldModel(fieldDescriptor) {
        if(fieldDescriptor.control && fieldDescriptor.control.id){
            return new FieldModel(fieldDescriptor, await fieldControlProvider.getFor(fieldDescriptor), null);
        }

        return new FieldModel(fieldDescriptor, null, null);
    }
}

class FieldModel {
    constructor(descriptor, controlType) {
        this.descriptor = descriptor;
        this.controlType = controlType;
    }
}

export default new FieldModelBuilder();