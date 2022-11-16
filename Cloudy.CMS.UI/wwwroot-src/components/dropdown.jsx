import { useEffect, useRef, useState } from "preact/hooks";
import ClickOutsideDetector from "./click-outside-detector"

const Dropdown = ({ text, className, button, children }) => {
  const [open, setOpen] = useState();
  const ref = useRef();

  useEffect(() => {
    const callback = event => {
      setOpen(false);
      event.stopPropagation();
    };

    if (ref.current) {
      ref.current.addEventListener('close-dropdown', callback);
    }

    return () => {
      if(ref.current){
        ref.current.removeEventListener('close-dropdown', callback);
      }
    }
  }, []);

  return <ClickOutsideDetector onClickOutside={() => setOpen(false)}>
    <div class={"dropdown d-inline-block" + (className ? ' ' + className : '')} ref={ref}>
      {button || <button class="btn btn-beta btn-sm dropdown-toggle" type="button" aria-expanded={open} onClick={() => setOpen(!open)}>{text}</button>}
      <div class={"dropdown-menu" + (open ? " show" : "")}>
        {open && children}
      </div>
    </div>
  </ClickOutsideDetector>;
}

export default Dropdown;