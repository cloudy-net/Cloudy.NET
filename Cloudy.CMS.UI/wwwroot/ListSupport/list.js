import html from '../html.js';

const List = props => html`
    <cloudy-ui-list>${props.children}<//>
`;

export default List;