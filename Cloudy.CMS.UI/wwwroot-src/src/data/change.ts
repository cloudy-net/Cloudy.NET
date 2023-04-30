type Change = {
  $type: string,
  path: string,
  date: number,
  key?: string, // lists
  type?: string, // block type changes
  value?: any, // simple changes
};

export default Change;