import closeDropdown from "./close-dropdown";

export default ({ href, target, onClick, icon, text, active, nbsp, keepOpen, ellipsis }) => <a
  className={"dropdown-item" + (active ? " active" : "")}
  href={href}
  onClick={event => {
    if (onClick) {
      onClick(event);
    }

    if (!keepOpen) {
      closeDropdown(event.target);
    }
  }}
  target={target}
  tabIndex="0"
>
  {icon && <div className="dropdown-item-icon">{icon}</div>}
  <div className={"dropdown-item-text" + (ellipsis ? " ellipsis" : "") + (nbsp ? ` nbsp` : "")}>{text}</div>
</a>;