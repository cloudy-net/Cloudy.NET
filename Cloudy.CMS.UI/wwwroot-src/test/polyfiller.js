import { LocalStorage } from "node-localstorage";

global.localStorage = new LocalStorage('./test/localStorage');

export default true;