import { VNode } from "preact";
import closeDropdown from "./close-dropdown";

export default ({ href, target, onClick, icon, text, active, nbsp, keepOpen, ellipsis, disabled }: { href?: string, target?: string, onClick?: (event: MouseEvent) => void, icon?: VNode<any>, text?: string, active?: boolean, nbsp?: boolean, keepOpen?: boolean, ellipsis?: boolean, disabled?: boolean }) => <a
  className={"dropdown-item" + (active ? " active" : "")}
  href={href}
  onClick={(event: MouseEvent) => {
    if (onClick) {
      onClick(event);
    }

    if (!keepOpen) {
      closeDropdown(event.target);
    }
  }}
  target={target}
  tabIndex={0}
>
  {icon && <div className="dropdown-item-icon">{icon}</div>}
  <div className={"dropdown-item-text" + (ellipsis ? " ellipsis" : "") + (nbsp ? ` nbsp` : "") + (disabled ? ` disabled` : "")}>{text}</div>
</a>;