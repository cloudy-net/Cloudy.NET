export default ({ href, icon, text }) => <a className="dropdown-item" href={href} target="_blank" tabIndex="0">
  <div className="dropdown-item-icon">{icon}</div>
  <div className="dropdown-item-text">{text}</div>
</a>;