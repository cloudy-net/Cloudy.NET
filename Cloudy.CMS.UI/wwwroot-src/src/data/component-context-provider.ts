
class componentContextProvider {
  getIdentifier(path: string) { return `cld-${path}`; }
}

export default new componentContextProvider();