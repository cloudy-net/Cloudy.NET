import { LocalStorage } from "node-localstorage";

export default () => {
  global.localStorage = new LocalStorage('./.test/localStorage');
  global.localStorage.clear();
  global.window = global.window || {};
};