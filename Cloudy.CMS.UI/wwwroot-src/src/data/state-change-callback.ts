import State from "./state";

type StateChangeCallback = (state: State | null) => void;

export default StateChangeCallback;