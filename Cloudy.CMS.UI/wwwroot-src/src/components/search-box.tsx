import debounce from "../util/debounce.js";
import { useEffect, useMemo, useRef } from 'preact/hooks';
import Search from "../assets/icon-search.svg";

export default ({ callback, autoFocus }: { callback: (search:string) => void, autoFocus?: true }) => {
  const debouncedResults = useMemo(() => {
    return debounce((event:InputEvent) => callback((event.target as HTMLInputElement)?.value), 250);
  }, []);

  useEffect(() => {
    return () => { // on unmount
      debouncedResults.cancel();
    };
  });

  const ref = useRef<HTMLInputElement>(null);

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