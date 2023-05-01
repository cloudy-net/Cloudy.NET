import { LocalStorage } from "node-localstorage";

global.localStorage = new LocalStorage('./.test/localStorage');
global.localStorage.clear();
global.window = global.window || {};

export default true;