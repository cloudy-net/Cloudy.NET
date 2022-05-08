import html from "../../util/html.js";
import Button from "../button/button.js";
import PopupMenu from "../popup-menu/popup-menu.js";

function ContextMenu({ children }){
    return html`
        <${PopupMenu} buttonClass=cloudy-ui-context-menu-button>
            ${children}
        <//>
    `;
}

export default ContextMenu;