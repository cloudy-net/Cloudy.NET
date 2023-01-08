import debounce from "../util/debounce.js";
import { html, useEffect, useMemo, useRef } from "../preact-htm/standalone.module.js";

export default ({ callback, floating, small }) => {
  const debouncedResults = useMemo(() => {
    return debounce(event => callback(event.target.value), 250);
  }, []);

  useEffect(() => {
    return () => { // on unmount
      debouncedResults.cancel();
    };
  });

  const ref = useRef(null);

  useEffect(() => {
    if (!ref.current) {
      return;
    }

    ref.current.focus();
  }, []);

  if (floating) {
    return html`<div class="form-floating list-page-search">
      <input class="form-control" type="text" onInput=${debouncedResults} ref=${ref} />
      <label>Search</label>
    </div>`;
  }

  return html`<input class=${"form-control" + (small ? " form-control-sm" : "")} type="text" placeholder="Search" onInput=${debouncedResults} ref=${ref} />`;
};