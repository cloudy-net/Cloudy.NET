
const groupBy = <T extends { [property: string]: string }>(array: T[], property: string): { [key: string]: T[] } =>
  array.reduce((grouped: { [key: string]: T[] }, element: T) => ({
    ...grouped,
    [element[property]]: [...(grouped[element[property]] || []), element]
  }), {});

export default groupBy;