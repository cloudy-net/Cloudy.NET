import debounce from "lodash.debounce";
import { useEffect, useMemo, useRef } from "@preact-htm";

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
    return <div class="form-floating list-page-search">
      <input class="form-control" type="text" onInput={debouncedResults} ref={ref} />
      <label>Search</label>
    </div>;
  }

  return <input class={"form-control" + (small ? " form-control-sm" : "")} type="text" placeholder="Search" onInput={debouncedResults} ref={ref} />;
};