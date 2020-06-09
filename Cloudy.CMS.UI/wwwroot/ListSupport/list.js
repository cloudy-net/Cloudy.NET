import ListItem from "./list-item.js";



/* LIST */

class List {
    constructor() {
        this.element = document.createElement('cloudy-ui-list');
    }

    addItem(argument) {
        if (argument instanceof ListItem) {
            argument.appendTo(this.element);
        } else {
            var item = new ListItem();

            argument(item);

            item.appendTo(this.element);
        }

        return this;
    }

    addSubHeader(text) {
        var header = document.createElement('cloudy-ui-list-sub-header');
        header.innerText = text;
        this.element.append(header);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default List;