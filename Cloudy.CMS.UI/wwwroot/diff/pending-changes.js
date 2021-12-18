import html from '../util/html.js';
import Blade from '../components/blade/blade.js';
import { useEffect, useState } from '../lib/preact.hooks.module.js';
import changeTracker from '../edit-content/change-tracker.js';
import ListItem from '../components/list/list-item.js';

function Item(props) {
    return html`
        <${ListItem} text=${props.change.contentId}/>
    `;
}

function ShowDiff() {
    const [items, setItems] = useState();

    useEffect(() => {
        setItems(changeTracker.pendingChanges.map(change => {
            return change;
            //    const name = nameGetter.getNameOf(content, contentType);
            //    const changedCount = change.changedFields.length;
            //    const subText = change.remove ? 'Slated for removal' : change.contentId ? `Changed fields: ${changedCount}` : `New ${contentType.lowerCaseName}`;

            //    list.addItem(new ListItem().setText(name).setSubText(subText).onClick(async () => {
            //        const contentType = await contentTypeProvider.get(change.contentTypeId);
            //        this.app.addBladeAfter(new PendingChangesDiffBlade(this.app, change, contentType), this);
            //    }));
        }));
    });

    return html`
        <${Blade} title='Pending changes'>
            ${items ? items.length ? items.map(item => html`<${Item} change=${item}/>`) : 'No more pending changes' : null}
        <//>
    `;
}

export default ShowDiff;