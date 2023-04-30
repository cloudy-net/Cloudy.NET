export default (a: any[] | null | undefined, b: any[] | null | undefined) => {
  if (a == null && b == null) {
    return true;
  }

  if (a == null) {
    return false;
  }

  if (b == null) {
    return false;
  }

  if (!Array.isArray(a) || !Array.isArray(b)) {
    return false;
  }

  if (a.length != b.length) {
    return false;
  }

  return a.every((ai, i) => ai === b[i]);
};