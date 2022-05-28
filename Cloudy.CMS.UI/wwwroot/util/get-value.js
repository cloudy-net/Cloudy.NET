const getValue = (object, path) => {
    let value = object;

    while(path.length){
        if(!value){
            return;
        }

        value = value[path[0]];

        path = path.splice(1);
    }

    return value;
};

export default getValue;