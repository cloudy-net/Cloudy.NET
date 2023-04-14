import { useEffect, useState } from "preact/hooks";
import { usePopper } from "react-popper";
import ClickOutsideDetector from "./click-outside-detector"
import { createRef } from "preact";

const Dropdown = ({ className, contents, children }) => {
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

  return <div class="dropdown" ref={ref}>
    <ClickOutsideDetector onClickOutside={() => setOpen(false)}>
      {<button className={className} type="button" aria-expanded={open} ref={setReferenceElement} onClick={() => setOpen(!open)}>{contents}</button>}

      {open && <div className="dropdown-menu" ref={setPopperElement} style={styles.popper} {...attributes.popper}>
        {children}
      </div>}
    </ClickOutsideDetector>
  </div>;
}

export default Dropdown;