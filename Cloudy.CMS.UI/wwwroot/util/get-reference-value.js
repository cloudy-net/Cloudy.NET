const getReferenceValue = (state, [...path]) => {
    let value = state.referenceValues;

    while(path.length){
        if(!value){
            return null;
        }

        if(path.length > 1){
            value = value[path[0]] ? value[path[0]].Value : null;
        } else {
            value = value[path[0]];
        }

        path = path.splice(1);
    }

    return value;
};

export default getReferenceValue;