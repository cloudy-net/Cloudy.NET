import fieldDescriptorProvider from './field-descriptor-provider.js';
import fieldControlProvider from './field-control-provider.js';
import FieldModel from './field-model.js';



/* FIELD MODEL BUILDER */

class FieldModelBuilder {
    async getFieldModels(formId) {
        var fieldDescriptors = await fieldDescriptorProvider.getFor(formId);

        var fieldModelPromises = fieldDescriptors.map(fieldDescriptor => this.getFieldModel(fieldDescriptor));

        return await Promise.all(fieldModelPromises);
    }

    async getFieldModel(fieldDescriptor) {
        if (fieldDescriptor.embeddedFormId) {
            var fieldModels = await this.getFieldModels(fieldDescriptor.embeddedFormId);

            if (fieldDescriptor.control) {
                var fieldControl = await fieldControlProvider.getFor(fieldDescriptor);
                return new FieldModel(fieldDescriptor, fieldControl, fieldModels);
            }

            return new FieldModel(fieldDescriptor, null, fieldModels);
        }

        var fieldControl = await fieldControlProvider.getFor(fieldDescriptor);

        return new FieldModel(fieldDescriptor, fieldControl, null);
    }
}

export default new FieldModelBuilder();