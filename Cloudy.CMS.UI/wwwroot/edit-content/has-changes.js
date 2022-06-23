const hasChanges = state => state.simpleChanges?.length || state.arrayChanges?.length;

export default hasChanges;