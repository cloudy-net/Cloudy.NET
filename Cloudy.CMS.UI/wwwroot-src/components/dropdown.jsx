import { useEffect, useState } from "preact/hooks";
import { usePopper } from "react-popper";
import ClickOutsideDetector from "./click-outside-detector"
import { createRef } from "preact";
import { ReactComponent as Caret } from "../assets/caret-vertical.svg";

const Dropdown = ({ className, contents, children, fullWidth }) => {
  const ref = createRef();
  const [open, setOpen] = useState();
  const [referenceElement, setReferenceElement] = useState(null);
  const [popperElement, setPopperElement] = useState(null);
  const { styles, attributes } = usePopper(referenceElement, popperElement, {});

  useEffect(() => {
    const callback = event => {
      setOpen(false);
      event.stopPropagation();
    };

    if (ref.current) {
      ref.current.addEventListener("close-dropdown", callback);
    }

    return () => {
      if (ref.current) {
        ref.current.removeEventListener("close-dropdown", callback);
      }
    }
  }, [open]);

  return <div className={"dropdown" + (fullWidth ? " full-width" : "")} ref={ref}>
    <ClickOutsideDetector onClickOutside={() => setOpen(false)}>
      {<button className={className || "dropdown-button"} type="button" aria-expanded={open} ref={setReferenceElement} onClick={() => setOpen(!open)}>
        {!className ? <span className="dropdown-button-text">{contents}</span> : contents}
        {!className ? <Caret className="dropdown-button-caret" /> : ''}
      </button>}
      {open && <div className="dropdown-menu" ref={setPopperElement} style={styles.popper} {...attributes.popper}>
        {children}
      </div>}
    </ClickOutsideDetector>
  </div>;
}

export default Dropdown;