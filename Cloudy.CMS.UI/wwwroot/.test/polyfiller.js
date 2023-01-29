import { LocalStorage } from "node-localstorage";

global.localStorage = new LocalStorage('./.test/localStorage');
global.localStorage.clear();

export default true;