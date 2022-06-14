const hasChanges = state => state.simpleChanges?.length || state.arrayAdds?.length;

export default hasChanges;