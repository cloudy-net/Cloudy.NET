
export default (array, property) => array && array.length && property && array.reduce((grouped, element) => ({
    ...grouped,
    [element[property]]: [...(grouped[element[property]] || []), element]
  }), {}) || [];