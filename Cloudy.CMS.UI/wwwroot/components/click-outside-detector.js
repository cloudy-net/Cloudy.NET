import { html, useEffect, useRef } from "../preact-htm/standalone.module.js";

export default ({ onClickOutside, children }) => {
    const ref = useRef(null);
  
    useEffect(() => {
      const callback = event => {
        if (!ref.current) {
          return;
        }
        if (!event.target.isConnected) {
          return;
        }
        if (ref.current == event.target || ref.current.contains(event.target)) {
          return;
        }
        onClickOutside();
      };
      document.addEventListener('click', callback);
      return () => document.removeEventListener('click', callback);
    }, []);
  
    return html`<div style="display: inline-block;" ref=${ref}>${children}</div>`;
  };