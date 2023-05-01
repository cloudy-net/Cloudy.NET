type Change = {
  $type: string,
  path: string,
  date: number,
  key?: string, // lists
  type?: string | null, // block type changes
  value?: any, // simple changes
};

export default Change;