


/* LIST */

class List {
    constructor() {
        this.element = document.createElement('poetry-ui-list');
    }

    addItem(configurator) {
        var item = new ListItem();

        configurator(item);

        item.appendTo(this.element);

        return this;
    }

    addSubHeader(text) {
        var header = document.createElement('poetry-ui-list-sub-header');
        header.innerText = text;
        this.element.append(header);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

class ListItem {
    constructor() {
        this.element = document.createElement('poetry-ui-list-item');

        this.element.tabIndex = 0;

        this.element.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            this.element.click();
        });

        this.callbacks = {
            click: [],
        };

        this.element.addEventListener('click', () => this.triggerClick());
    }

    triggerClick() {
        this.callbacks.click.forEach(callback => callback());
    }

    onClick(callback) {
        this.callbacks.click.push(callback);

        return this;
    }

    setText(value) {
        this.text = document.createElement('poetry-ui-list-item-text');
        this.text.innerText = value;
        this.element.append(this.text);
    }

    setSubText(value) {
        this.subText = document.createElement('poetry-ui-list-item-sub-text');
        this.subText.innerText = value;
        this.element.append(this.subText);
    }

    setActive(value = true) {
        if (value) {
            this.element.classList.add('poetry-ui-active');
        } else {
            this.element.classList.remove('poetry-ui-active');
        }
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default List;