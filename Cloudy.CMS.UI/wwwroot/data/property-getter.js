import arrayEquals from "../util/array-equals.js";

class PropertyGetter {
    get(state, path) {
        if(!state.changedFields){
            return;
        }
        
        const changedField = state.changedFields.find(f => arrayEquals(f.path, path) && f.type == 'simple' && f.operation == 'set');

        if (changedField) {
            return changedField.value;
        }

        return state.referenceValues[path[0]];
    }
}

export default new PropertyGetter();