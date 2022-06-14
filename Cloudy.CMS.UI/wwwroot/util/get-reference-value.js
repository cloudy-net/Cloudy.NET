import fieldDescriptorProvider from '../edit-content/form/field-descriptor-provider.js';

const getReferenceValue = (state, [...path]) => {
    let fieldDescriptors = fieldDescriptorProvider.get(state.contentReference.contentTypeId);
    let value = state.referenceValues;

    while(path.length){
        const fieldName = path[0];
        const fieldDescriptor = fieldDescriptors.find(f => f.id == fieldName);
        path = path.splice(1);

        if(!value) {
            return null;
        }

        if(fieldDescriptor.embeddedFormId){
            fieldDescriptors = fieldDescriptorProvider.get(fieldDescriptor.embeddedFormId);
            
            value = value[fieldName];
            
            if(!value){
                return null;
            }
            
            value = value.Value;
        } else if(fieldDescriptor.isSortable){
            value = value[fieldName];
        } else {
            value = value[fieldName];
        }
    }

    return value;
};

export default getReferenceValue;