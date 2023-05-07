import { ComponentChildren } from "preact";
import { useEffect, useState } from "preact/hooks";
import { usePopper } from "react-popper";
import ClickOutsideDetector from "./click-outside-detector"
import { createRef } from "preact";
import Caret from "../assets/caret-vertical.svg";

const Dropdown = ({ className, contents, children, fullWidth, wideContent }: { className?: string, contents: string, children: ComponentChildren, fullWidth?: boolean, wideContent?: boolean }) => {
  const ref = createRef();
  const [open, setOpen] = useState(false);
  const [referenceElement, setReferenceElement] = useState<HTMLElement | null>(null);
  const [popperElement, setPopperElement] = useState<HTMLElement | null>(null);
  const { styles, attributes } = usePopper(referenceElement, popperElement, {});

  useEffect(() => {
    const callback = (event: MouseEvent) => {
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

  return <div className={"dropdown" + (fullWidth ? " fullwidth" : "")} ref={ref}>
    <ClickOutsideDetector onClickOutside={() => setOpen(false)} blockDisplay={!!fullWidth}>
      {<button className={className || "dropdown-button"} type="button" aria-expanded={open} ref={setReferenceElement} onClick={() => setOpen(!open)}>
        {!className ? <span className="dropdown-button-text">{contents}</span> : contents}
        {!className ? <Caret className="dropdown-button-caret" /> : ''}
      </button>}
      {open && <div className={"dropdown-menu" + (wideContent ? " wide" : "")} ref={setPopperElement} style={styles.popper} {...attributes.popper}>
        {children}
      </div>}
    </ClickOutsideDetector>
  </div>;
}

export default Dropdown;