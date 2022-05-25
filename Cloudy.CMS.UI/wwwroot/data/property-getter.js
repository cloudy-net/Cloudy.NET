import arrayEquals from "../util/array-equals.js";

class PropertyGetter {
    get(state, path) {
        if(!state.changedFields){
            return;
        }

        let value = path.length == 1 ? state.referenceValues[path[0]] : null;
        
        for(let change of state.changedFields.filter(f => arrayEquals(f.path, path))){
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
    }
}

export default new PropertyGetter();