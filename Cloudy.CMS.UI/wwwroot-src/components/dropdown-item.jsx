import closeDropdown from "./close-dropdown";

export default ({ href, target, onClick, icon, text }) => <a
  className="dropdown-item"
  href={href}
  onClick={event => {
    if(onClick) {
      onClick(event);
    }

    closeDropdown(event.target);
  }}
  target={target}
  tabIndex="0"
>
  {icon && <div className="dropdown-item-icon">{icon}</div>}
  <div className="dropdown-item-text">{text}</div>
</a>;