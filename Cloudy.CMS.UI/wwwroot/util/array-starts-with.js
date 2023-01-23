const arrayStartsWith = (a, b) => {
  if (a == null && b == null) {
      return false;
  }

  if (a == null) {
      return false;
  }

  if (b == null) {
      return false;
  }

  if(!Array.isArray(a) || !Array.isArray(b)){
      return false;
  }

  if(a.length < b.length){
      return false;
  }

  return b.every((bi, i) => bi === a[i]);
};

export default arrayStartsWith;