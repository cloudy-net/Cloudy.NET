import debounce from "../util/debounce.js";
import { useEffect, useMemo, useRef } from 'preact/hooks';

export default ({ callback, className }) => {
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

  return <input class={className} type="text" placeholder="Search" onInput={debouncedResults} ref={ref} />;
};