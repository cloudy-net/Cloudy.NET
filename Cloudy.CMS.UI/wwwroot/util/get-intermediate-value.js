import arrayEquals from "./array-equals.js";
import getValue from "./get-value.js";

const getIntermediateValue = (object, path, changes) => {
    let value = getValue(object, path);
    
    for(let change of changes.filter(c => arrayEquals(c.path, path))){
        if(change.type == 'simple'){
            if(change.operation == 'set'){
                value = change.value;
            }
        }

        if(change.type == 'array'){
            if(value === null){
                value = [];
            }
            
            if(change.operation == 'add'){
                value.push(change.value);
            }
        }
    }

    return value;
};

export default getIntermediateValue;