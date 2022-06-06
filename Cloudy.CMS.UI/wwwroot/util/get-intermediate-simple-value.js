import arrayEquals from "./array-equals.js";
import getReferenceValue from "./get-reference-value.js";

const getIntermediateSimpleValue = (state, path) => {
    let value = getReferenceValue(state, path);
    
    const change = state.simpleChanges.find(c => arrayEquals(c.path, path));

    if(change){
        value = change.value;
    }

    return value;
};

export default getIntermediateSimpleValue;