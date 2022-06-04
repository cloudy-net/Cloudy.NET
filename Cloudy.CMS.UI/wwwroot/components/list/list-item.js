import html from '../../util/html.js';

const ListItem = ({ active, disabled, onclick, image, text, subtext, badges, menu, link, target }) => html`
    <${link ? 'a' : 'cloudy-ui-list-item'} href=${link ? link : null} target=${target} class="${(active && 'cloudy-ui-active')} ${(disabled && 'cloudy-ui-disabled')} ${link && 'cloudy-ui-list-item'}">
        <cloudy-ui-list-item-content tabindex=0 onclick=${!disabled && onclick}>
            ${image && html`<img class=cloudy-ui-list-item-image src=${image} alt=''><//>`}
            <cloudy-ui-list-item-text-container>
                <cloudy-ui-list-item-text>${text}<//>
                ${subtext && html`<cloudy-ui-list-item-sub-text>${subtext}<//>`}
            <//>
            ${badges}
        <//>
        ${menu}
    <//>
`;

export default ListItem;
