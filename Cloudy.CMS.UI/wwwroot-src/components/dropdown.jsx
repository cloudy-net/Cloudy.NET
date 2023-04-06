import { useEffect, useState  } from "preact/hooks";
import { usePopper } from "react-popper";
import ClickOutsideDetector from "./click-outside-detector"

const Dropdown = ({ className, contents, children }) => {
  const [open, setOpen] = useState();
  const [referenceElement, setReferenceElement] = useState(null);
  const [popperElement, setPopperElement] = useState(null);
  const { styles, attributes } = usePopper(referenceElement, popperElement, {});

  useEffect(() => {
    const callback = event => {
      setOpen(false);
      event.stopPropagation();
    };

    if (referenceElement) {
      referenceElement.addEventListener("close-dropdown", callback);
    }

    return () => {
      if(referenceElement){
        referenceElement.removeEventListener("close-dropdown", callback);
      }
    }
  }, []);

  return <ClickOutsideDetector onClickOutside={() => setOpen(false)}>
      {<button className={className} type="button" aria-expanded={open} ref={setReferenceElement} onClick={() => setOpen(!open)}>{contents}</button>}

      {open && <div className="dropdown-menu" ref={setPopperElement} style={styles.popper} {...attributes.popper}>
        {children}
      </div>}
    </ClickOutsideDetector>;
}

export default Dropdown;