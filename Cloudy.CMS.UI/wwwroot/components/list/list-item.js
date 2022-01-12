import html from '../../util/html.js';

const ListItem = props => html`
    <cloudy-ui-list-item class="${(props.active && 'cloudy-ui-active')} ${(props.disabled && 'cloudy-ui-disabled')}">
        <cloudy-ui-list-item-content tabindex=0 onclick=${!props.disabled && props.onclick}>
            ${props.image && html`<img class=cloudy-ui-list-item-image src=${props.image} alt=''><//>`}
            <cloudy-ui-list-item-text-container>
                <cloudy-ui-list-item-text>${props.text}<//>
                ${props.subtext && html`<cloudy-ui-list-item-sub-text>${props.subtext}<//>`}
            <//>
        <//>
        ${props.menu}
    <//>
`;

export default ListItem;
