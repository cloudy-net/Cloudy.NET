import arrayEquals from "./array-equals.js";
import getValue from "./get-value.js";

const getIntermediateSimpleValue = (object, path, simpleChanges) => {
    let value = getValue(object, path);
    
    const change = simpleChanges.find(c => arrayEquals(c.path, path));

    if(change){
        value = change.value;
    }

    return value;
};

export default getIntermediateSimpleValue;