const getReferenceValue = (state, path) => {
    let value = state.referenceValues;

    
    let pathSegments = path.split('.');

    while(pathSegments.length){
        if(!value){
            return null;
        }

        if(pathSegments.length > 1){
            value = value[pathSegments[0]] ? value[pathSegments[0]].Value : null;
        } else {
            value = value[pathSegments[0]];
        }

        pathSegments = pathSegments.splice(1);
    }

    return value;
};

export default getReferenceValue;