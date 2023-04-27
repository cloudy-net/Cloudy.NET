import debounce from "../util/debounce.js";
import { useEffect, useMemo, useRef } from 'preact/hooks';
import { ReactComponent as Search } from "../assets/icon-search.svg";

export default ({ callback, className, autoFocus }) => {
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

    if (autoFocus) {
      ref.current.focus();
    }
  }, []);
  return <div class="search-box">
    <input class="search-box-input" type="text" placeholder="Search" onInput={debouncedResults} ref={ref} />
    <Search className="search-box-icon" />
  </div>;
};