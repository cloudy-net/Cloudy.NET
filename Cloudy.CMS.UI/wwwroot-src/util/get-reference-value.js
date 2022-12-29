const getReferenceValue = (state, [...path]) => {
    let value = state.referenceValues;

    while(path.length){
        if(!value){
            return null;
        }

        value = value[path[0]];

        path = path.splice(1);
    }

    return value;
};

export default getReferenceValue;