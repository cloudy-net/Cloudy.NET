import { LocalStorage } from "node-localstorage";

global.localStorage = new LocalStorage('./scratch');

export default true;